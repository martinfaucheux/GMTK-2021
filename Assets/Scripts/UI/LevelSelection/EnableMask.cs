using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMask : MonoBehaviour
{
    [SerializeField] MonoBehaviour _maskComponent;

    void Start()
    {
        _maskComponent.enabled = true;
    }
}
