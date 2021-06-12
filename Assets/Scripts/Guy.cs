using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy : Entity
{
    public Blob blob;

    protected override void Start()
    {
        base.Start();
    }

    public override bool CanCollide(){
        return isBlocking & (blob == null);
    }

    public override void OnCollide(Blob collidingBlob)
    {
        base.OnCollide(collidingBlob);
        isBlocking = false;
        Debug.Log("I am collided");
        collidingBlob.AbsorbGuy(this);
    }
}
