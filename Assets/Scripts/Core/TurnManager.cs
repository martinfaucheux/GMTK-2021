using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct CollisionCouple
{
    Entity interacted;
    Entity interacting;
}

// <summary>
// Keep track of what should happen within a turn
// </summary>
public class TurnManager : SingletonBase<TurnManager>
{

    public float moveCooldown
    {
        get
        {
            float timeSinceMove = Time.time - _lastMoveTime;
            return Mathf.Max(0f, GameManager.instance.actionDuration - timeSinceMove);
        }
    }

    public bool isReady
    {
        get
        {
            // return true if no registered descriptor are tweening
            return !_moveDescriptors.Any(ltd => LeanTween.isTweening(ltd));
        }
    }

    private float _lastMoveTime;
    private List<Blob> _controlledBlobs;
    private HashSet<Entity> _entitiesToMove;
    private Direction _turnDirection;
    private List<int> _moveDescriptors = new List<int>();

    void Start()
    {
        _controlledBlobs = new List<Blob>();
        PlayerController.OnGetCommand += TriggerMovement;
    }

    void OnDestroy()
    {
        PlayerController.OnGetCommand -= TriggerMovement;
    }

    private void TriggerMovement(Direction direction)
    {
        if (
            moveCooldown == 0
            && GameManager.instance.playerCanMove
            && isReady
        )
        {
            _lastMoveTime = Time.time;
            StartCoroutine(PlayTurn(direction));
        }
    }

    private IEnumerator PlayTurn(Direction direction)
    {
        _turnDirection = direction;
        _moveDescriptors = new List<int>();

        GameEvents.instance.StartOfTurnTrigger();
        List<(Entity, Entity)> collisionList = StartTurn();

        float maxMoveDuration = GetMaxMoveDuration();
        MoveTransforms();

        yield return new WaitForSeconds(maxMoveDuration);

        EndTurn(collisionList);
        GameEvents.instance.EndOfTurnTrigger();
    }

    private List<(Entity, Entity)> StartTurn()
    {

        _entitiesToMove = new HashSet<Entity>();
        List<(Entity, Entity)> collisionList = new List<(Entity, Entity)>();

        // fix inconsistancies in movements when having several controllable blobs
        List<Blob> controlledBlobsOrdered = Ordering.Sort(
            _controlledBlobs,
            blob => blob.GetMovementPriority(_turnDirection)
        );

        foreach (Blob blob in controlledBlobsOrdered)
        {
            (Vector2Int displacement, List<(Entity, Entity)> blobCollisionList) = blob.GetMovement(_turnDirection);

            if (displacement != Vector2Int.zero)
            {
                foreach (Guy guy in blob.guys)
                {
                    guy.matrixCollider.matrixPosition += displacement;
                    _entitiesToMove.Add(guy);
                }
            }

            // order the list with burger resolved at the end
            List<(Entity, Entity)> blobCollisionListOrdered = Ordering.Sort(
                blobCollisionList, // pass the actuall list
                (elt) => elt.Item2.GetResolveOrder() // function used to order
            );

            foreach ((Entity interactingEntity, Entity interactedEntity) in blobCollisionListOrdered)
            {
                interactedEntity.PreInteract(interactingEntity);
                collisionList.Add((interactingEntity, interactedEntity));
            }
        }
        return collisionList;
    }

    private void MoveTransforms()
    {

        if (_entitiesToMove.Any())
            AudioManager.instance?.Play("Zoom");

        foreach (Entity entity in _entitiesToMove)
        {
            GameObject objectToMove = entity.gameObject;
            Vector3 realWorldPos = entity.matrixCollider.GetRealPos();
            float moveDuration = GetMoveDuration(entity);
            _moveDescriptors.Add(LeanTween.move(objectToMove, realWorldPos, moveDuration).id);
        }
    }

    private void EndTurn(List<(Entity, Entity)> collisionList)
    {
        if (_entitiesToMove.Any())
            CameraShake.instance.ShakeOnce(_turnDirection);

        foreach ((Entity interactingEntity, Entity interactedEntity) in collisionList)
            interactedEntity.Interact(interactingEntity);
    }

    public void Register(Blob controlledBlob)
    {
        // TurnManager Start method must run before Blob's
        _controlledBlobs.Add(controlledBlob);
    }

    public void Unregister(Blob controlledBlob)
    {
        _controlledBlobs.Remove(controlledBlob);
    }

    private static float GetMoveDuration(Entity entity)
    {

        Vector2Int originalMatrixPos = CollisionMatrix.instance.GetMatrixPos(entity.transform);
        Vector2Int displacement = entity.matrixPosition - originalMatrixPos;
        int distance = Mathf.Max(Mathf.Abs(displacement.x), Mathf.Abs(displacement.y));

        if (distance == 0)
            return 0f;

        int maxDistance = CollisionMatrix.instance.maxDistance;
        float maxDuration = GameManager.instance.actionDuration;
        float minDuration = 0.33f * maxDuration;

        float duration = minDuration + (maxDuration - minDuration) * ((float)distance / (float)maxDistance);

        // always less than maxDuration
        return duration;
    }

    private float GetMaxMoveDuration()
    {
        float maxDuration = 0f;
        foreach (Entity entity in _entitiesToMove)
        {
            float moveDuration = GetMoveDuration(entity);
            if (moveDuration > maxDuration)
                maxDuration = moveDuration;
        }
        return maxDuration;
    }
}
