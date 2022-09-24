using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenInit : MonoBehaviour
{
    public int maxSimultaneousTweens = 800;

    void Awake()
    {
        LeanTween.init(maxSimultaneousTweens);
    }
}
