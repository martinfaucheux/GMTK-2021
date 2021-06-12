using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YeahUI : MonoBehaviour
{
    private Image _imageComponent;

    [SerializeField] float popDuration = 0.2f;

    void Start()
    {
        _imageComponent = GetComponent<Image>();
        GameEvents.instance.onWin += PopImage;
    }

    void OnDestroy(){
        GameEvents.instance.onWin -= PopImage;
    }

    private void PopImage(){
        Vector3 targetScale = new Vector3(1f,1f,1f);
        LeanTween.scale(gameObject, targetScale, popDuration).setEaseOutBack();
    }

    

}
