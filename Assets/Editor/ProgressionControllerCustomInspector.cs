using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ProgressionController))]
public class ProgressionControllerCustomInspector : Editor
{
    private ProgressionController t;

    private void OnEnable() {
        t = target as ProgressionController;
        // SceneView.duringSceneGui -= OnScene;
        // SceneView.duringSceneGui += OnScene;
    }

    // void OnScene(SceneView sceneview)
    // {
    //     if (t.showSceneBounds)
    //     {
    //         DrawMatrixBounds();
    //     }
    // }


    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefaultInspector();

        if (GUILayout.Button("Set grid size"))
        {
            t.SetGridAssetSize();
        }

        // if (GUI.changed)
        // {
        //     EditorUtility.SetDirty(t);
        //     EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
        // }
    }
}