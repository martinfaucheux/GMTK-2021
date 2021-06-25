using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Dictionary<Entity, Vector2Int> _forecastMatrixPositions;

    void Awake(){
        CheckSingleton();
    }

    void Start(){
        _controlledBlobs = new List<Blob>();
        _forecastMatrixPositions = new Dictionary<Entity, Vector2Int>();
        PlayerController.OnGetCommand += TriggerMovement;
    }

    void OnDestroy(){
        PlayerController.OnGetCommand -= TriggerMovement;
    }

    private void TriggerMovement(Direction direction){
        if (moveCooldown == 0 & GameManager.instance.playerCanMove){
            StartCoroutine(PlayTurn(direction));
        }       
    }

    private IEnumerator PlayTurn(Direction direction){
        List<(Entity, Entity)> collisionList = StartTurn(direction);
        MoveTransforms();
        yield return new WaitForSeconds(GameManager.instance.actionDuration);
        EndTurn(collisionList);
    }

    private List<(Entity, Entity)> StartTurn(Direction direction){
        // logic of the turn should lay here

        _forecastMatrixPositions = new Dictionary<Entity, Vector2Int>();
        List<(Entity, Entity)> collisionList = new List<(Entity, Entity)>();

        foreach(Blob blob in _controlledBlobs){
            (Vector2Int displacement, List<(Entity, Entity)> blobCollisionList) = blob.GetMovement(direction);

            foreach(Guy guy in blob.guys){
                guy.matrixCollider.matrixPosition += displacement;
            }

            foreach((Entity interactingEntity, Entity interactedEntity) in blobCollisionList){
                interactedEntity.PreInteract(interactingEntity);
                collisionList.Add((interactingEntity, interactedEntity));
            }

            AddMovement(blob, displacement);
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

        foreach(KeyValuePair<Entity, Vector2Int> item in _forecastMatrixPositions){
            GameObject objectToMove= item.Key.gameObject;
            Vector3 realWorldPos = CollisionMatrix.instance.GetRealWorldPosition(item.Value);
            LeanTween.move(objectToMove, realWorldPos, moveDuration);
        }
    }

    private void AddMovement(Blob blob, Vector2Int displacement){
        // add movement to forecast map for all guys of the blob
        foreach(Guy guy in blob.guys){
            AddMovement(guy, displacement);
        }
    }

    private void AddMovement(Entity entity, Vector2Int displacement){
        // add movement to forecast map for a specific entity
        if(!_forecastMatrixPositions.ContainsKey(entity)){
            _forecastMatrixPositions[entity] = new Vector2Int(0, 0);
        }
        _forecastMatrixPositions[entity] += displacement;
    }

    public void Register(Blob controlledBlob){
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
}
