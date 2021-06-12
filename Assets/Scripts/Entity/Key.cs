using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Entity
{
    public override void Interact(Blob collidingBlob)
    {
        base.Interact(collidingBlob);
        GameManager.instance.Win();
    }
}
