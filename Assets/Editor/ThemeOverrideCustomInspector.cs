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

        if (GUILayout.Button("Apply Theme"))
        {
            ApplyTheme(t);
        }
    }

    public static void ApplyTheme(ThemeOverride t)
    {
        List<Object> changedObjects = new List<Object>();

        changedObjects.Concat(ChangeBackgroundColor(t));
        changedObjects.Concat(ChangeGrassColor(t));
        changedObjects.Concat(ChangeSeaMaterial(t));

        List<Object> uiObjects = ChangeUIElementColor(t);
        changedObjects.Concat(uiObjects);

        List<GameObject> prefabs = ChangeObstacleObjects(t);
        changedObjects.Concat(prefabs);

        foreach (Object uiObject in uiObjects)
            EditorUtility.SetDirty(uiObject);

        PrefabUtility.RecordPrefabInstancePropertyModifications(GetCommonObject(t));
        EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
    }

    [MenuItem("Tools / Apply Theme")]
    public static void ApplyTheme()
    {
        ThemeOverride themeOverride = GameObject.FindObjectOfType<ThemeOverride>();
        ApplyTheme(themeOverride);
    }

    private static List<Object> ChangeBackgroundColor(ThemeOverride t)
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

    private static List<Object> ChangeGrassColor(ThemeOverride t)
    {
        List<Object> changedObjects = new List<Object>();

        SpriteRenderer[] grassSpriteRenderers = t.grassHolder.GetComponentsInChildren<SpriteRenderer>();
        Color grassColor = t.overrideData.grassColor;
        grassColor.a = 1f;

        foreach (SpriteRenderer spriteRenderer in grassSpriteRenderers)
        {
            spriteRenderer.color = grassColor;
            changedObjects.Add(spriteRenderer);
        }
        return changedObjects;
    }
    private static List<Object> ChangeSeaMaterial(ThemeOverride t)
    {
        t.seaSpriteRenderer.material = t.overrideData.seaMaterial;
        return new List<Object>() { t.seaSpriteRenderer };
    }

    private static List<Object> ChangeUIElementColor(ThemeOverride t)
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

    private static List<GameObject> ChangeObstacleObjects(ThemeOverride t)
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

    private static void DisableSand(ThemeOverride t)
    {
        if (!t.overrideData.allowSand)
            t.sandRandomizer.Delete();
    }

    private static GameObject GetCommonObject(ThemeOverride t) => t.gameManager.transform.parent.gameObject;

}