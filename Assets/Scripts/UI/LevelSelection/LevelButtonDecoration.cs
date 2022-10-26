using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonDecoration : MonoBehaviour
{
    [SerializeField] float _enableRatio = 0.2f;

    void Start()
    {
        foreach (Transform childTransform in transform)
        {
            childTransform.gameObject.SetActive(Random.Range(0f, 1f) < _enableRatio);
        }
    }
}
