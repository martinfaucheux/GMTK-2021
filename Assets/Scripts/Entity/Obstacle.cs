using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Entity{
    
    [SerializeField] float trembleAmplitude = 0.1f;
    [SerializeField] float trembleTime = 0.05f;

    public override void Interact(Blob blob){
        base.Interact(blob);
        TrembleAnimation();
    }

    private void TrembleAnimation(){
        Vector3 offset = new Vector3(trembleAmplitude, 0f, 0f);
        LeanTween.move(
            gameObject,
            transform.position + offset,
            trembleTime
        ).setLoopPingPong(2);
    }
}
