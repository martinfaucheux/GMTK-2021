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

        if (moveCooldown == 0 & GameManager.instance.playerCanMove){
            AttemptMove(direction);
        }       
    }

    private void AttemptMove(Direction direction)
    {
        _lastMoveTime = Time.time;
        _blob.AttemptMove(direction);
    }

}
