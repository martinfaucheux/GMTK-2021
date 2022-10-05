using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Entity
{
    [SerializeField] GameObject explosionPrefab;


    public override void PreInteract(Entity entity)
    {
        base.PreInteract(entity);
        Burger.ResolveExplosion();
    }



    public override void Interact(Entity entity)
    {
        base.Interact(entity);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Burger.AnimateExplosion();
        CameraShake.instance?.Shake();
        Destroy(gameObject);
    }

}
