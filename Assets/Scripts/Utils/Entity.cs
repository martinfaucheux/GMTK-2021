using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public MatrixCollider matrixCollider{get; private set;}
    public bool isInteractable = true; // whether Interact should be run when colliding
    public bool isBlocking = true; // whether this blocks movement

    public virtual bool CanCollide(){
        return isBlocking;
    }

    public virtual void Interact(Blob collidingBlob){

    }

    protected virtual void Start()
    {
        matrixCollider = GetComponent<MatrixCollider>();
    }
}
