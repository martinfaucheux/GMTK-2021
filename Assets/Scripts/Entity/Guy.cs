using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy : Entity
{
    public Blob blob;

    [SerializeField] GameObject bridgeSkinPrefab;

    public override void Interact(Entity entity)
    {
        base.Interact(entity);

        Guy interactingGuy = entity as Guy;
        if (interactingGuy != null && interactingGuy.blob != null){
            interactingGuy.blob.Absorb(this);
            BuildSkinBridges(blob);
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

    private void BuildSkinBridges(Blob blob){
        foreach(Guy otherGuy in blob.guys){
            Vector2Int distToOtherGuy = otherGuy.matrixPosition - matrixPosition;
            if (distToOtherGuy.sqrMagnitude < 1.01 ){
                Vector3 bridgeOffset = 0.5f * new Vector3(distToOtherGuy.x, distToOtherGuy.y, 0f);
                Vector3 skinBridgePosition = transform.position +  bridgeOffset;
                Instantiate(bridgeSkinPrefab, skinBridgePosition, Quaternion.identity, blob.skinBridgePoolTransform);
            }
        }
    }

    public void Extract(){
        if(blob != null){
            blob.guys.Remove(this);
        }
    }
}
