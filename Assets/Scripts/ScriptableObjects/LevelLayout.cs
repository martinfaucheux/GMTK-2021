using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;


[CreateAssetMenu(fileName = "HealthPotion", menuName = "Scriptable Objects / Level Layout")]
public class LevelLayout : ScriptableObject
{
    public Array2DInt levelGrid;
}
