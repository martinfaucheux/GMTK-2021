using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public MatrixCollider matrixCollider{get; private set;}

    public bool isBlocking {
        get{
            return matrixCollider.IsBlocking;
        }
    }

    public virtual void OnCollide(Blob collidingBlob){

    }

    protected virtual void Start()
    {
        matrixCollider = GetComponent<MatrixCollider>();
    }
}
