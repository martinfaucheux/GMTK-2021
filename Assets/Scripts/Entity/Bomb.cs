using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Entity
{
    [SerializeField] GameObject explosionPrefab;

    protected override void Start(){
        base.Start();
    }

    public override void Interact(Blob collidingBlob)
    {
        base.Interact(collidingBlob);
        Debug.Log("KABOUM");

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Burger.DestroyAll();
        Destroy(gameObject);
    }
}
