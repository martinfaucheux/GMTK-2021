using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Blob : MonoBehaviour
{

    [SerializeField] float moveSpeed = 10;

    
    public Transform guyPoolTransform; 
    public Transform skinBridgePoolTransform; 

    [SerializeField] float bloupScaleRatio = 1.1f;
    
    
    private CollisionMatrix _collisionMatrix;
    
    // private bool _isMoving = false;
    public List<Guy> guys {get; private set;}
    
    private List<Entity> interactedToResolve;
    private List<Entity> collidedToResolve;

    void Start()
    {
        // add initial collider to the blob
        guys = new List<Guy>();

        Guy[] initGuys = guyPoolTransform.GetComponentsInChildren<Guy>();
        foreach(Guy guy in initGuys){
            AbsorbGuy(guy);
        }
        
        interactedToResolve = new List<Entity>();
        collidedToResolve = new List<Entity>();
    }

    public (Vector2Int, List<Entity>) GetMovement(Direction direction){
        // return the maximum possible displacement as well the list of collided objects
        int distance = 0;
        Vector2Int dirVect = direction.ToPos();

        List<Entity> collidedEntities = new List<Entity>();

        bool isDisplacementPossible = true; // main condition to get out of loop
        bool isDisplacementStopped = false; // if a entity pins down the blob
        while(isDisplacementPossible & !isDisplacementStopped){

            // store entities collided at this distance iteration
            List<Entity> iterationEntityList = new List<Entity>();

            foreach(Guy guy in guys){                
                Vector2Int positionToCheck = guy.matrixCollider.matrixPosition + (distance + 1) * dirVect;
                bool isValidPosition = CollisionMatrix.instance.IsValidPosition(positionToCheck);
                bool isEntityBlocking = false;

                GameObject objectAtPosition = CollisionMatrix.instance.GetObjectAtPosition(positionToCheck);
                if (objectAtPosition != null){
                    Entity entityComponent = objectAtPosition.GetComponent<Entity>();

                    if (entityComponent != null){
                        isEntityBlocking = entityComponent.isBlocking;
                        if(isEntityBlocking){
                            collidedToResolve.Add(entityComponent);
                        }

                        if (entityComponent.isInteractable){
                            iterationEntityList.Add(entityComponent);
                            isDisplacementStopped = entityComponent.isStopMovement;
                        }
                    }
                }
                isDisplacementPossible &= (isValidPosition & !isEntityBlocking);
            }

            // trigger Interact method only for entities that have interactWhenOutOfReach
            // in case the movement has been blocked
            foreach(Entity collidedEntity in iterationEntityList){
                if(isDisplacementPossible | collidedEntity.interactWhenOutOfReach){
                    collidedEntities.Add(collidedEntity);
                }
            }
            
            // only add if displacement is possible
            distance += isDisplacementPossible ? 1 : 0;
        }
        Vector2Int maxDisplacement = distance * dirVect;
        return (maxDisplacement, collidedEntities);
    }

    public bool AttemptMove(Direction direction){
        (Vector2Int displacement, List<Entity> collidedEntities) = GetMovement(direction);
        interactedToResolve = collidedEntities;
        AnimateMove(displacement);

        if(displacement.sqrMagnitude > 0f)
            AudioManager.instance?.Play("Zoom");

        return true;
    }

    public void AnimateMove(Vector2Int displacement){
        // _isMoving = true;
        foreach(Guy guy in guys){
            guy.matrixCollider.matrixPosition += displacement;
        }
        float moveDuration = Mathf.Min(GameManager.instance.actionDuration, displacement.magnitude * 1f / moveSpeed);

        Vector3Int v3Dsiplacement = (Vector3Int) displacement;
        Vector3 newRealWorldPos = transform.position + v3Dsiplacement;

        LeanTween.move(gameObject, newRealWorldPos, moveDuration).setOnComplete(ResolveCollision);
    }

    public void AbsorbGuy(Guy guy){
        guys.Add(guy);
        guy.blob = this;
        guy.transform.SetParent(guyPoolTransform);
    }

    private void ResolveCollision(){      

        if (interactedToResolve.Any()){
            // trigger Interact method
            foreach(Entity interactedEntity in interactedToResolve){
                interactedEntity.Interact(this);
            }

            // bloup animation
            Vector3 targetScale = bloupScaleRatio * transform.localScale;
            LeanTween.scale(gameObject, targetScale, 0.2f).setLoopPingPong(1);

            // wow animation
            foreach(Guy guy in guys){
                GameEvents.instance.BlobCollisionTrigger(guy.gameObject.GetInstanceID());
            }
        }

        foreach(Entity collidedEntity in collidedToResolve){
            collidedEntity.Collide(this);
        }

        collidedToResolve = new List<Entity>();
        interactedToResolve = new List<Entity>();
    }
}
