using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Scriptable Objects / Theme")]
public class ThemeOverrideData : ScriptableObject
{
    public Color backgroundColor;
    public Material seaMaterial;
    public GameObject obstaclePrefab;
    public Color uiElementsColor;
}
