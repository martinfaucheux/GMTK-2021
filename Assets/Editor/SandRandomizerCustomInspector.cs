using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(SandRandomizer))]
public class SandRandomizerCustomInspector : Editor
{

    private SandRandomizer t;

    private void OnEnable()
    {
        t = target as SandRandomizer;
    }

    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefaultInspector();

        if (GUILayout.Button("Add Sand"))
        {
            InstantiateSand();
        }

        if (GUILayout.Button("Delete Sand"))
        {
            DeleteSand();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(t);
            EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
        }
    }

    private void InstantiateSand()
    {
        GameObject prefab = t.sandPlacementPrefabs[Random.Range(0, t.sandPlacementPrefabs.Length)];
        Quaternion rotation = Quaternion.Euler(0, 0, 90 * Random.Range(0, 4));

        GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

        newObject.transform.position = t.transform.position;
        newObject.transform.rotation = rotation;
        newObject.transform.SetParent(t.transform);
    }
    private void DeleteSand()
    {
        foreach (Transform childTransform in t.transform)
        {
            if (childTransform.name.StartsWith(t.containerName))
                DestroyImmediate(childTransform.gameObject);
        }
    }

}