using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Blob : MonoBehaviour
{

    [SerializeField] bool isControlled = false;
    public Transform guyPoolTransform;
    public List<Guy> guys { get; private set; }

    void Start()
    {
        // add initial collider to the blob
        guys = new List<Guy>();

        Guy[] initGuys = guyPoolTransform.GetComponentsInChildren<Guy>();
        foreach (Guy guy in initGuys)
            Absorb(guy);

        if (isControlled)
            TurnManager.instance.Register(this);
    }

    public (Vector2Int, List<(Entity, Entity)>) GetMovement(Direction direction)
    {
        // return the maximum possible displacement as well the list of collided objects
        int distance = 0;
        Vector2Int dirVect = direction.ToPos();

        List<(Entity, Entity)> collidedEntities = new List<(Entity, Entity)>();

        bool isDisplacementPossible = true; // main condition to get out of loop
        bool isDisplacementStopped = false; // if a entity pins down the blob

        if (!guys.Any())
        {
            // this happens if the blob is emptied withing the turn
            isDisplacementPossible = false;
        }

        while (isDisplacementPossible & !isDisplacementStopped)
        {

            // store entities collided at this distance iteration
            List<(Entity, Entity)> iterationEntityList = new List<(Entity, Entity)>();

            foreach (Guy guy in guys)
            {
                Vector2Int positionToCheck = guy.matrixCollider.matrixPosition + (distance + 1) * dirVect;
                bool isValidPosition = CollisionMatrix.instance.IsValidPosition(positionToCheck);
                bool isEntityBlocking = false;

                Entity entityComponent = GetEntityAtPosition(positionToCheck);
                if (entityComponent != null)
                {
                    isEntityBlocking = entityComponent.IsBlocking(guy);

                    if (entityComponent.CanInteract(guy))
                    {
                        iterationEntityList.Add((guy, entityComponent));
                        isDisplacementStopped = entityComponent.isStopMovement;
                    }
                }

                isDisplacementPossible &= (isValidPosition & !isEntityBlocking);
            }

            // trigger Interact method only for entities that have interactWhenOutOfReach
            // in case the movement has been blocked
            foreach ((Entity collidingEntity, Entity collidedEntity) in iterationEntityList)
            {
                if (isDisplacementPossible | collidedEntity.interactWhenOutOfReach)
                    collidedEntities.Add((collidingEntity, collidedEntity));
            }

            // only add if displacement is possible
            distance += isDisplacementPossible ? 1 : 0;
        }
        Vector2Int maxDisplacement = distance * dirVect;
        return (maxDisplacement, collidedEntities);
    }

    private static Entity GetEntityAtPosition(Vector2Int matrixPosition)
    {
        // helper for collision resolution
        GameObject objectAtPosition = CollisionMatrix.instance.GetObjectAtPosition(matrixPosition);
        if (objectAtPosition != null)
        {
            Entity entity = objectAtPosition.GetComponent<Entity>();
            if (entity != null)
                return entity;
        }
        return null;
    }

    public void Absorb(Guy guy)
    {
        guy.Extract(); // remove the guy from his current blob
        guys.Add(guy);
        guy.blob = this;
        guy.transform.SetParent(guyPoolTransform);
    }

    public void Absorb(Blob absorbedBlob)
    {
        // mark absorbed blob as not controlled
        TurnManager.instance.Unregister(absorbedBlob);
        absorbedBlob.isControlled = false;

        // absorb remaining guys
        // use a copy of the list because it will be modified
        foreach (Guy guy in new List<Guy>(absorbedBlob.guys))
            Absorb(guy);
    }

    public void Amaze()
    {
        foreach (Guy guy in guys)
            guy.Amaze();
    }

    public void Remove(Guy guy) => guys.Remove(guy);

    public int GetMovementPriority(Direction direction)
    {
        int minDistance = CollisionMatrix.instance.maxDistance;
        foreach (Guy guy in guys)
        {
            int distance = GetDistanceToBorder(guy.matrixPosition, direction);
            if (distance < minDistance)
                minDistance = distance;
        }
        return minDistance;
    }

    private static int GetDistanceToBorder(Vector2Int matrixPosition, Direction direction)
    {
        int result = 0;
        Vector2Int maxPos = CollisionMatrix.instance.matrixSize;
        Vector2Int dirVect = direction.ToPos();
        switch (direction.ToString())
        {
            case "UP":
                result = maxPos.y - matrixPosition.y;
                break;
            case "DOWN":
                result = matrixPosition.y;
                break;
            case "LEFT":
                result = matrixPosition.x;
                break;
            case "RIGHT":
                result = maxPos.x - matrixPosition.x;
                break;
            default:
                break;
        }
        return result;
    }
}
