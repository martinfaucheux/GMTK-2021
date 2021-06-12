using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Blob : MonoBehaviour
{

    [SerializeField] float moveSpeed = 10;

    [SerializeField] Transform guyPoolTransform; 
    private CollisionMatrix _collisionMatrix;
    
    private bool _isMoving = false;
    private List<Guy> guys;
    
    private List<Entity> collidedToResolve;

    void Start()
    {
        // add initial collider to the blob
        Guy[] initGuys = guyPoolTransform.GetComponentsInChildren<Guy>(); 
        guys = initGuys.OfType<Guy>().ToList(); // convert to list
        
        collidedToResolve = new List<Entity>();
    }

    public (Vector2Int, List<Entity>) GetMovement(Direction direction){
        // return the maximum possible displacement as well the list of collided objects
        int distance = 0;
        Vector2Int dirVect = direction.ToPos();

        List<Entity> collidedEntities = new List<Entity>();

        bool isDisplacementPossible = true;
        while(isDisplacementPossible){
            foreach(Guy guy in guys){                
                Vector2Int positionToCheck = guy.matrixCollider.matrixPosition + (distance + 1) * dirVect;
                bool isValidPosition = CollisionMatrix.instance.IsValidPosition(positionToCheck);
                bool isEntityBlocking = false;

                GameObject objectAtPosition = CollisionMatrix.instance.GetObjectAtPosition(positionToCheck);
                if (objectAtPosition != null){
                    Entity entityComponent = objectAtPosition.GetComponent<Entity>();
                    if (entityComponent != null & entityComponent.CanCollide()){
                        collidedEntities.Add(entityComponent);
                        isEntityBlocking = entityComponent.isBlocking;
                    }
                }
                isDisplacementPossible &= (isValidPosition & !isEntityBlocking);
            }
            
            // only add if displacement is possible
            distance += isDisplacementPossible ? 1 : 0;
        }
        Vector2Int maxDisplacement = distance * dirVect;
        return (maxDisplacement, collidedEntities);
    }

    public bool AttemptMove(Direction direction){
        (Vector2Int displacement, List<Entity> collidedEntities) = GetMovement(direction);
        collidedToResolve = collidedEntities;
        Debug.Log("collided to resolve: " + collidedToResolve.ToString());
        AnimateMove(displacement);

        return true;
    }

    public void AnimateMove(Vector2Int displacement){
        _isMoving = true;
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
        _isMoving = false;
        foreach(Entity collidedEntity in collidedToResolve){
            collidedEntity.OnCollide(this);
        }
        collidedToResolve = new List<Entity>();
    }
}
