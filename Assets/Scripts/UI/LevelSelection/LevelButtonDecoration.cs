using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonDecoration : MonoBehaviour
{
    [SerializeField] float _enableRatio = 0.2f;
    private static int RANDOM_SEED = 42;
    private static System.Random _randomizer;

    void Start()
    {
        foreach (Transform childTransform in transform)
            childTransform.gameObject.SetActive(GetRandomValue() < _enableRatio);
    }

    private static float GetRandomValue()
    {
        if (_randomizer == null)
            _randomizer = new System.Random(RANDOM_SEED);

        return (float)_randomizer.NextDouble();
    }
}
