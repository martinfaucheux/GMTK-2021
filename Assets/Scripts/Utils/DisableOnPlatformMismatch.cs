using UnityEngine;
using System.Linq;
using UnityEditor;
public class DisableOnPlatformMismatch : MonoBehaviour
{
    public RuntimePlatform[] allowedPlatforms;
    public bool debug = false;
    void OnEnable()
    {
        if (!IsPlatformAllowed())
            gameObject.SetActive(false);
    }

    private bool IsPlatformAllowed()
    {

#if UNITY_EDITOR

        if (debug)
        {
            return true;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            BuildTarget activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            if (activeBuildTarget == BuildTarget.Android)
            {
                return allowedPlatforms.Contains(RuntimePlatform.Android);
            }
            else
            {
                return true;
            }
        }
#endif
        return allowedPlatforms.Contains(Application.platform);
    }
}