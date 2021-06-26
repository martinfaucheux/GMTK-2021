using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Blob : MonoBehaviour
{

    [SerializeField] bool isControlled = false;
    [SerializeField] float moveSpeed = 10;

    public Transform guyPoolTransform; 
    public Transform skinBridgePoolTransform; 

    [SerializeField] float bloupScaleRatio = 1.1f;
        
    // private bool _isMoving = false;
    public List<Guy> guys {get; private set;}

    private List<(Entity, Entity)> interactedToResolve;

    private Vector3 _initScale;

    private bool _doWow;

    void Start()
    {
        _initScale = transform.localScale;
        // add initial collider to the blob
        guys = new List<Guy>();

        Guy[] initGuys = guyPoolTransform.GetComponentsInChildren<Guy>();
        foreach(Guy guy in initGuys){
            Absorb(guy);
        }

        if (isControlled){
            TurnManager.instance.Register(this);
        }
    }

    public (Vector2Int, List<(Entity, Entity)>) GetMovement(Direction direction){
        // return the maximum possible displacement as well the list of collided objects
        int distance = 0;
        Vector2Int dirVect = direction.ToPos();

        List<(Entity, Entity)> collidedEntities = new List<(Entity, Entity)>();

        bool isDisplacementPossible = true; // main condition to get out of loop
        bool isDisplacementStopped = false; // if a entity pins down the blob
        while(isDisplacementPossible & !isDisplacementStopped){

            // store entities collided at this distance iteration
            List<(Entity, Entity)> iterationEntityList = new List<(Entity, Entity)>();

            foreach(Guy guy in guys){                
                Vector2Int positionToCheck = guy.matrixCollider.matrixPosition + (distance + 1) * dirVect;
                bool isValidPosition = CollisionMatrix.instance.IsValidPosition(positionToCheck);
                bool isEntityBlocking = false;

                Entity entityComponent = GetEntityAtPosition(positionToCheck);
                if (entityComponent != null){
                    isEntityBlocking = entityComponent.IsBlocking(guy);

                    if (entityComponent.CanInteract(guy)){
                        iterationEntityList.Add((guy, entityComponent));
                        isDisplacementStopped = entityComponent.isStopMovement;
                    }
                }

                isDisplacementPossible &= (isValidPosition & !isEntityBlocking);
            }

            // trigger Interact method only for entities that have interactWhenOutOfReach
            // in case the movement has been blocked
            foreach((Entity collidingEntity, Entity collidedEntity) in iterationEntityList){
                if(isDisplacementPossible | collidedEntity.interactWhenOutOfReach){
                    collidedEntities.Add((collidingEntity, collidedEntity));
                }
            }
            
            // only add if displacement is possible
            distance += isDisplacementPossible ? 1 : 0;
        }
        Vector2Int maxDisplacement = distance * dirVect;
        return (maxDisplacement, collidedEntities);
    }

    private static Entity GetEntityAtPosition(Vector2Int matrixPosition){
        // helper for collision resolution
        GameObject objectAtPosition = CollisionMatrix.instance.GetObjectAtPosition(matrixPosition);
        if (objectAtPosition != null){
            Entity entity = objectAtPosition.GetComponent<Entity>();
            if (entity != null){    
                return entity;
            }
        }
        return null;
    }

    public void Absorb(Guy guy){
        guy.Extract(); // remove the guy from his current blob
        guys.Add(guy);
        guy.blob = this;
        guy.transform.SetParent(guyPoolTransform);
    }

    public void Absorb(Blob otherBlob){
        // transfert skin bridges to new blob
        TransferSkinBridges(otherBlob);

        // absorb remaining guys
        // use a copy of the list because it will be modified
        foreach(Guy guy in new List<Guy>(otherBlob.guys)){
            Absorb(guy);
        }
        Destroy(otherBlob.gameObject);
    }

    private void TransferSkinBridges(Blob otherBlob){
        // copy list of child transforms first
        List<Transform> childTransforms = new List<Transform>();
        foreach (Transform childTransform in otherBlob.skinBridgePoolTransform)
        { 
            childTransforms.Add(childTransform);
        }

        // iterate over copy because child transforms will change
        foreach(Transform skinBridgeTransform in childTransforms){
            skinBridgeTransform.SetParent(this.skinBridgePoolTransform);
        }
    }

    private void ResolveCollision(){

        // Set of unique encountered blobs
        HashSet<Blob> encounteredBlobs = new HashSet<Blob>();
        // keep track if need to do wow animation
        bool doWow = false;

        // order the list with burger resolved at the end
        List<(Entity, Entity)> interactedToResolveOrdered = Sort(
            interactedToResolve, // pass the actuall list
            (elt) => elt.Item2.GetResolveOrder() // function used to order
        );

        // trigger Interact method
        foreach((Entity interactingEntity, Entity interactedEntity) in interactedToResolveOrdered){
            // keep trace of blobs to merge
            Guy encounteredGuy = interactedEntity as Guy;
            if (encounteredGuy != null){
                doWow = true;
                if(encounteredGuy.blob != null){
                    encounteredBlobs.Add(encounteredGuy.blob);
                }
            }
            // do wow if collided entity is a burger
            doWow |= (interactedEntity as Burger) != null;

            interactedEntity.Interact(interactingEntity);
        }
        
        foreach(Blob encounteredBlob in encounteredBlobs){
            Absorb(encounteredBlob);
        }

        // if any guy collided
        if(doWow){
            // DoWow();
        }
        interactedToResolve = new List<(Entity, Entity)>();
    }

    public static List<T> Sort<T>(
        List<T> source, Func<T, int> sortFunction, bool asc = true
    ) where T : new() {
        // used to sort the list of objects to resolve
        return asc ? source.OrderBy(x => sortFunction.Invoke(x)).ToList() : source.OrderByDescending(x => sortFunction.Invoke(x)).ToList();
    }
}
