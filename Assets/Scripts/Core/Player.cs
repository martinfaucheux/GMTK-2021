using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // To be DEPRECATED in favor of TurnManager

    public float moveCooldown{
        get{
            float timeSinceMove = Time.time - _lastMoveTime;
            return Mathf.Max(0f, GameManager.instance.actionDuration - timeSinceMove);
        }
    }

    private MatrixCollider _matrixCollider;
    private Blob _blob;
    private float _lastMoveTime;

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
