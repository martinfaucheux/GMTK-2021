using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YeahUI : MonoBehaviour
{
    [SerializeField] Image yeahImageComponent;
    [SerializeField] RectTransform fadeImageTransform;


    [SerializeField] float popDuration = 0.2f;
    [SerializeField] float finalFadeAlpha = 0.33f;

    void Start()
    {
        GameEvents.instance.onWin += PopImage;
    }

    void OnDestroy(){
        GameEvents.instance.onWin -= PopImage;
    }

    private void PopImage(){
        Vector3 targetScale = new Vector3(1f,1f,1f);
        LeanTween.scale(gameObject, targetScale, popDuration).setEaseOutBack();
        LeanTween.alpha(fadeImageTransform, finalFadeAlpha, popDuration);
        LeanTween.rotate(gameObject, Vector3.forward * 7, 4 * popDuration).setLoopPingPong();
    }

    

}
