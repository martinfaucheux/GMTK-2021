using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy : Entity
{
    public Blob blob;

    [SerializeField] GameObject bridgeSkinPrefab;

    public override void PreInteract(Entity entity){
        base.PreInteract(entity);
        
        Guy interactingGuy = entity as Guy;
        if (interactingGuy != null && interactingGuy.blob != null){
            interactingGuy.blob.Absorb(this);
        }
    }

    public override void Interact(Entity entity)
    {
        base.Interact(entity);

        Guy interactingGuy = entity as Guy;
        if (interactingGuy != null && interactingGuy.blob != null){
            BuildSkinBridges(interactingGuy);
        }
    }

    public override bool IsBlocking(Entity otherEntity){
        Guy otherGuy = otherEntity as Guy;
        if(otherGuy != null && otherGuy.blob != null){
            // if it's a guy, he is blocked if he is in a different blob
            return (this.blob != otherGuy.blob);
        }
        return base.IsBlocking(otherEntity);
    }

    public override bool CanInteract(Entity otherEntity)
    {
        Guy otherGuy = otherEntity as Guy;
        if(otherGuy != null && otherGuy.blob != null){
            // can only interact if guy is in another blob
            return (this.blob != otherGuy.blob);
        }
        return base.CanInteract(otherEntity);
    }

    private void BuildSkinBridges(Guy interactingGuy){
        Vector3 skinBridgePosition = (interactingGuy.transform.position +  transform.position) / 2f;
        Instantiate(bridgeSkinPrefab, skinBridgePosition, Quaternion.identity, transform);
    }

    public void Extract(){
        if(blob != null){
            blob.guys.Remove(this);
        }
    }
}
