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
    private bool _isMoving = false;
    private float _inverseMoveTime;
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
        _inverseMoveTime = 1f / GameManager.instance.actionDuration;
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

        Vector2Int newMatrixPosition = _matrixCollider.GetMaxInLinePosition(direction);
        Debug.Log(newMatrixPosition);

        if (newMatrixPosition != _matrixCollider.matrixPosition){
            Move(newMatrixPosition);
        }

        // GameObject collidingObject = _matrixCollider.GetObjectInDirection(direction);

        // bool canMove = true;

        // if (collidingObject != null)
        // {
        //     MatrixCollider otherCollider = collidingObject.GetComponent<MatrixCollider>();
        //     canMove = !otherCollider.IsBlocking;

        //     if (otherCollider == null)
        //     {
        //         Debug.LogError(otherCollider.ToString() + ": colliding but no collider found");
        //         return;
        //     }
        // }     
        // // Check that direction is valid and that object is able to move
        // if (_matrixCollider.IsValidDirection(direction) & canMove)
        // {
        //     Move(direction);
        // }
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

    private bool Move(Direction direction)
    {
        // update collider position
        _matrixCollider.matrixPosition += direction.ToPos();
        Debug.Log("New Matrix Pos: " + _matrixCollider.matrixPosition.ToString());

        // update real position
        Vector3 realPosStart = transform.position;
        // need to use a CollisionMatrix method instead
        Vector3 realPosEnd = realPosStart + CollisionMatrix.instance.GetRealWorldVector(direction);

        LeanTween.move(gameObject, realPosEnd, GameManager.instance.actionDuration);
        
        // StartCoroutine(SmoothMovement(realPosEnd));

        // return True if we successfuly move
        return true;
    }

    private void SetNotMoving(){
        _isMoving = false;
    }

    // protected IEnumerator SmoothMovement(Vector3 endPos)
    // {
    //     // sqr for the remaining distance
    //     // = distance between the current position and the end position
    //     float sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

    //     _isMoving = true;
    //     while (sqrRemainingDistance > float.Epsilon)
    //     {
    //         // move the rigidbody moveUnits units toward the end position
    //         float moveUnits = _inverseMoveTime * Time.deltaTime;
    //         Vector3 newPosition = Vector3.MoveTowards(transform.position, endPos, moveUnits);
    //         transform.position = newPosition;
    //         sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;
    //         // wait for a frame before reevalue the conditions of the loop
    //         yield return null;

    //     }

    //     // set at the correct place at the end
    //     transform.position = endPos;
    //     _isMoving = false;
    // }
}
