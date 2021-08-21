using UnityEngine;
using System.Linq;

public class DisableOnPlatformMismatch : MonoBehaviour
{
    public RuntimePlatform[] allowedPlatforms;
    void OnEnable()
    {
        if (!IsPlatformAllowed())
            gameObject.SetActive(false);
    }

    private bool IsPlatformAllowed()
    {
        return allowedPlatforms.Contains(Application.platform);
    }
}