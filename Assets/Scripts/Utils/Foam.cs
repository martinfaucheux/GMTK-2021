using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foam : MonoBehaviour
{
    [SerializeField] float amplitude;
    [SerializeField] float period;

    void Start()
    {
        Vector3 scale = transform.localScale + amplitude / 10 * Vector3.one;
        LeanTween.scale(gameObject, scale, period / 2).setLoopPingPong();    
    }

}
