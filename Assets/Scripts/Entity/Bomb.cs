using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Entity
{
    protected override void Start(){
        base.Start();
    }

    public override void Interact(Blob collidingBlob)
    {
        base.Interact(collidingBlob);
        // TODO:
    }
}
