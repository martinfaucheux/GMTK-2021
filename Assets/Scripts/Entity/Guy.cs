using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy : Entity
{
    public Blob blob;

    [SerializeField] GameObject bridgeSkinPrefab;

    public override void Interact(Blob collidingBlob)
    {
        base.Interact(collidingBlob);
        isBlocking = false;
        isInteractable = false;
        collidingBlob.AbsorbGuy(this);
        BuildSkinBridges(blob);
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
}
