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
        PlayerController.OnGetCommand += StartTurn;
    }

    private void StartTurn(Direction direction){
        // TODO: logic of the turn should lay here

        _forecastMatrixPositions = new Dictionary<Entity, Vector2Int>();

        foreach(Blob blob in _controlledBlobs){
            (Vector2Int displacement, List<(Entity, Entity)> collidedEntities) = blob.GetMovement(direction);

            // resolve interactions here

            AddMovement(blob, displacement);
        }

        MoveTransforms();

        // TODO: wait end of movement


    }

    private void EndTurn(){
        // TODO: play animations and end of Turn 

        // check end of game and stuff
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
