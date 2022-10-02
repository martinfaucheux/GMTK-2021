using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Scriptable Objects / Theme")]
public class ThemeOverrideData : ScriptableObject
{
    public Color backgroundColor;
    public Color grassColor;
    public Material seaMaterial;
    public GameObject obstaclePrefab;
    public Color uiElementsColor;
    public bool allowSand = true;
}
