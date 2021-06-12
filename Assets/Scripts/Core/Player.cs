using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveCooldown{
        get{
            float timeSinceMove = Time.time - _lastMoveTime;
            return Mathf.Max(0f, GameManager.instance.actionDuration - timeSinceMove);
        }
    }
    public float moveSpeed = 10;

    private MatrixCollider _matrixCollider;
    private Blob _blob;
    private bool _isMoving = false;
    private float _lastMoveTime;
    public static Player instance;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a CollisionMatrix.
            Destroy(gameObject);

    }

    void Start(){
        _matrixCollider = GetComponent<MatrixCollider>();
        _blob = GetComponent<Blob>();
        PlayerController.OnGetCommand += TriggerMovement;

    }
    private void OnDestroy(){
        PlayerController.OnGetCommand -= TriggerMovement;
    }

    private void TriggerMovement(Direction direction){
        // Debug.Log("Attempt move CD:" + moveCooldown.ToString());

        if (moveCooldown == 0){
            AttemptMove(direction);
        }       
    }

    private void AttemptMove(Direction direction)
    {
        _lastMoveTime = Time.time;
        _blob.AttemptMove(direction);
    }

    private void MoveBlob(Vector2Int displacement){

    }

    private bool Move(Vector2Int newMatrixPos){

        // compute move duration
        float distance = (_matrixCollider.matrixPosition - newMatrixPos).magnitude;
        float moveDuration = Mathf.Min(GameManager.instance.actionDuration, distance * 1f / moveSpeed);

        // update position
        _matrixCollider.matrixPosition = newMatrixPos;
        Vector3 newRealWorldPos = _matrixCollider.GetRealPos();

        LeanTween.move(gameObject, newRealWorldPos, moveDuration).setOnComplete(SetNotMoving);

        return true;
    }

    private void SetNotMoving(){
        _isMoving = false;
    }

}
