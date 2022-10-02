using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Linq;

[CustomEditor(typeof(ThemeOverride))]
public class ThemeOverrideCustomInspector : Editor
{
    private static string OBSTACLE_NAME = "Obstacle";

    private ThemeOverride t;


    private void OnEnable()
    {
        t = target as ThemeOverride;
    }

    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefaultInspector();

        if (GUILayout.Button("Change Theme"))
        {
            ChangeTheme();
        }
    }

    private void ChangeTheme()
    {
        List<Object> changedObjects = new List<Object>();

        changedObjects.Concat(ChangeBackgroundColor());
        changedObjects.Concat(ChangeSeaMaterial());
        changedObjects.Concat(ChangeUIElementColor());

        List<GameObject> prefabs = ChangeObstacleObjects();
        changedObjects.Concat(prefabs);

        EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
    }

    private List<Object> ChangeBackgroundColor()
    {
        List<Object> changedObjects = new List<Object>();

        SpriteRenderer[] bgSpriteRenderers = t.backgroundHolder.GetComponentsInChildren<SpriteRenderer>();
        Color bgColor = t.overrideData.backgroundColor;
        bgColor.a = 1f;

        foreach (SpriteRenderer spriteRenderer in bgSpriteRenderers)
        {
            spriteRenderer.color = bgColor;
            changedObjects.Add(spriteRenderer);
        }
        return changedObjects;
    }

    private List<Object> ChangeSeaMaterial()
    {
        t.seaSpriteRenderer.material = t.overrideData.seaMaterial;
        return new List<Object>() { t.seaSpriteRenderer };
    }

    private List<Object> ChangeUIElementColor()
    {
        List<Object> changedObjects = new List<Object>();

        Color color = t.overrideData.uiElementsColor;
        color.a = 1f;
        foreach (MonoBehaviour component in t.uiElements)
        {
            Image uiImage = component as Image;
            TextMeshProUGUI text = component as TextMeshProUGUI;
            if (uiImage != null)
                uiImage.color = color;
            else if (text != null)
                text.color = color;
            else
                Debug.LogError("Unhandled component: " + component.gameObject.name, component);

            changedObjects.Add(component);
        }
        return changedObjects;
    }

    private List<GameObject> ChangeObstacleObjects()
    {
        List<GameObject> changedObjects = new List<GameObject>();

        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject rootObject in rootObjects)
        {
            if (!rootObject.name.StartsWith(OBSTACLE_NAME))
                continue;

            Vector3 position = rootObject.transform.position;

            GameObject newObject = PrefabUtility.InstantiatePrefab(t.overrideData.obstaclePrefab) as GameObject;
            newObject.transform.position = position;

            DestroyImmediate(rootObject);
            changedObjects.Add(rootObject);
            changedObjects.Add(newObject);
        }
        return changedObjects;
    }

}