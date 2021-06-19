using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public MatrixCollider matrixCollider{get; private set;}
    [SerializeField] protected bool isInteractable = true; // whether Interact should be run when colliding
    public bool isBlocking = true; // whether this blocks movement
    public bool isStopMovement = false; // wheter displacement is allowed on the case but it can't go further

    public bool interactWhenOutOfReach = false;

    public string collidingSoundName;

    public bool playSound = true;

    public Vector2Int matrixPosition{
        get{return matrixCollider.matrixPosition;}
    }

    public virtual void Interact(Entity collidingEntity){
        PlaySound();
    }

    protected virtual void Start()
    {
        matrixCollider = GetComponent<MatrixCollider>();
    }

    private void PlaySound(){
        if(playSound && collidingSoundName != ""){
            AudioManager.instance?.Play(collidingSoundName);
        }
    }

    public virtual bool CanInteract(Entity otherEntity){
        return isInteractable;
    }
}
