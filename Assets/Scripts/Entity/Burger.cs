using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burger : Entity
{
    [SerializeField] SpriteRenderer spriteRenderer;

    public override void Interact(Blob collidingBlob)
    {
        base.Interact(collidingBlob);
        spriteRenderer.enabled = false;
        GameManager.instance.Win();
    }
}
