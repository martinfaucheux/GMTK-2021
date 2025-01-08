using UnityEngine;
using System.Linq;
using UnityEditor;
public class DisableOnPlatformMismatch : MonoBehaviour
{
    public RuntimePlatform[] allowedPlatforms;
    public bool debug = false;
    public void OnEnable()
    {
        gameObject.SetActive(IsPlatformAllowed());
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
            bool isBuildForAndroid = activeBuildTarget == BuildTarget.Android;
            bool shouldEnableForAndroid =  allowedPlatforms.Contains(RuntimePlatform.Android);
            return isBuildForAndroid == shouldEnableForAndroid;
        }
#endif
        return allowedPlatforms.Contains(Application.platform);
    }
}