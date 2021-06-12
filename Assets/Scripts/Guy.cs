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

    public override void OnCollide(Blob collidingBlob)
    {
        base.OnCollide(collidingBlob);
        Debug.Log("I am collided");
        collidingBlob.AbsorbGuy(this);
    }
}
