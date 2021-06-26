using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public struct CollisionCouple{
    Entity interacted;
    Entity interacting;
}

public class TurnManager : MonoBehaviour
{
    // Keep track of what should happen within a turn
    
    public static TurnManager instance;
    
    public float moveCooldown{
        get{
            float timeSinceMove = Time.time - _lastMoveTime;
            return Mathf.Max(0f, GameManager.instance.actionDuration - timeSinceMove);
        }
    }

    private float _lastMoveTime;
    private List<Blob> _controlledBlobs;
    private HashSet<Entity> _entitiesToMove;

    void Awake(){
        CheckSingleton();
    }

    void Start(){
        _entitiesToMove = new HashSet<Entity>();
        _controlledBlobs = new List<Blob>();
        PlayerController.OnGetCommand += TriggerMovement;
    }

    void OnDestroy(){
        PlayerController.OnGetCommand -= TriggerMovement;
    }

    private void TriggerMovement(Direction direction){
        if (moveCooldown == 0 & GameManager.instance.playerCanMove){
            _lastMoveTime = Time.time;
            StartCoroutine(PlayTurn(direction));
        }       
    }

    private IEnumerator PlayTurn(Direction direction){
        Debug.Log("Play Turn");
        List<(Entity, Entity)> collisionList = StartTurn(direction);
        MoveTransforms();
        yield return new WaitForSeconds(GameManager.instance.actionDuration);
        EndTurn(collisionList);
        GameEvents.instance.EndOfTurnTrigger();
    }

    private List<(Entity, Entity)> StartTurn(Direction direction){
        // logic of the turn should lay here

        List<(Entity, Entity)> collisionList = new List<(Entity, Entity)>();

        foreach(Blob blob in _controlledBlobs){
            (Vector2Int displacement, List<(Entity, Entity)> blobCollisionList) = blob.GetMovement(direction);

            foreach(Guy guy in blob.guys){
                guy.matrixCollider.matrixPosition += displacement;
                _entitiesToMove.Add(guy);
            }

            // order the list with burger resolved at the end
            List<(Entity, Entity)> blobCollisionListOrdered = Sort(
                blobCollisionList, // pass the actuall list
                (elt) => elt.Item2.GetResolveOrder() // function used to order
            );

            foreach((Entity interactingEntity, Entity interactedEntity) in blobCollisionListOrdered){
                interactedEntity.PreInteract(interactingEntity);
                collisionList.Add((interactingEntity, interactedEntity));
            }
        }
        return collisionList;
    }

    private void EndTurn(List<(Entity, Entity)> collisionList){
        foreach((Entity interactingEntity, Entity interactedEntity) in collisionList){
            interactedEntity.Interact(interactingEntity);
        }
    }

    private void MoveTransforms(){
        float moveDuration = GameManager.instance.actionDuration;

        foreach(Entity entity in _entitiesToMove){
            GameObject objectToMove= entity.gameObject;
            Vector3 realWorldPos = entity.matrixCollider.GetRealPos();
            LeanTween.move(objectToMove, realWorldPos, moveDuration);
        }
    }

    public void Register(Blob controlledBlob){
        // TurnManager Start method must run before Blob's
        _controlledBlobs.Add(controlledBlob);
    }

    private void CheckSingleton(){
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a CollisionMatrix.
            Destroy(gameObject);
    }

    public static List<T> Sort<T>(
        List<T> source, Func<T, int> sortFunction, bool asc = true
    ) where T : new() {
        // used to sort the list of objects to resolve
        return asc ? source.OrderBy(x => sortFunction.Invoke(x)).ToList() : source.OrderByDescending(x => sortFunction.Invoke(x)).ToList();
    }
}
