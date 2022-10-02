using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float initCameraSize = 13f;
    public float targetCameraSize = 10.5f;

    void Start()
    {
        Camera.main.orthographicSize = initCameraSize;
        Zoom();
    }

    public void Zoom()
    {
        LeanTween.value(
            gameObject,
            SetSize,
            Camera.main.orthographicSize,
            targetCameraSize,
            LevelLoader.instance.transitionDuration
        ).setEaseInOutQuad();
    }

    private static void SetSize(float camSize) => Camera.main.orthographicSize = camSize;
}
