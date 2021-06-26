using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Entity
{
    [SerializeField] GameObject explosionPrefab;

    protected override void Start(){
        base.Start();
    }

    public override void PreInteract(Entity entity){
        base.PreInteract(entity);
        Burger.DisableAll();

    }

    public override void Interact(Entity entity)
    {
        base.Interact(entity);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Burger.PlayBurnAnimation();
        Destroy(gameObject);
    }
}
