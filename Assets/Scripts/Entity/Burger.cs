using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burger : Entity
{
    [SerializeField] SpriteRenderer spriteRenderer;

    protected override void Start(){
        base.Start();
        GameManager.instance.burgerToCollect ++;
    }

    public override void Interact(Blob collidingBlob)
    {
        base.Interact(collidingBlob);
        spriteRenderer.enabled = false;
        GameManager.instance.burgerToCollect --;
        GameManager.instance.CheckWinCondition();
    }
}
