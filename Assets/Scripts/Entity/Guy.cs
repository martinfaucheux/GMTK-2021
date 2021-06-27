using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy : Entity
{
    public Blob blob;
    [SerializeField] GameObject bridgeSkinPrefab;

    private bool _doWow = false;

    protected override void Start()
    {
        base.Start();
        GameEvents.instance.onEndOfTurn += OnEndOfTurn;
    }

    void OnDestroy(){
        GameEvents.instance.onEndOfTurn -= OnEndOfTurn;
    }

    public override void PreInteract(Entity entity){
        base.PreInteract(entity);
        
        Guy interactingGuy = entity as Guy;
        if (interactingGuy != null && interactingGuy.blob != null){
            Blob interactingblob = interactingGuy.blob;
            if(blob != null){
                if(blob != interactingGuy.blob){
                    interactingblob.Absorb(blob);
                }
            }
            else {
                interactingblob.Absorb(this);
            }
            interactingblob.Amaze();
        }
    }

    public override void Interact(Entity entity){
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
        if(
            otherGuy != null
            && otherGuy.blob != null
        ){
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
            blob.Remove(this);
            blob = null;
        }
    }

    public void Amaze() => _doWow = true;

    private void OnEndOfTurn(){
        if(_doWow){
            DoWow();
        }
        _doWow = false;
    }

    private void DoWow(){
        Vector3 targetScale = 1.1f * Vector3.one;
        // bloup animation
        LeanTween.scale(gameObject, targetScale, 0.1f).setLoopPingPong(1);
        // wow animation
        GameEvents.instance.BlobCollisionTrigger(gameObject.GetInstanceID());
    }
}
