using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerTransition : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    private Vector2 _initPosition;
    private bool _isSlideIn = false;

    private RectTransform _rectTransform;

    private float moveDuration{
        get{ return LevelLoader.instance.transitionDuration / 2;}
    }

    void Start()
    {
        _rectTransform = (RectTransform) transform;
        _initPosition = _rectTransform.anchoredPosition;

        GameEvents.instance.onFadeOut += SlideIn;
        GameEvents.instance.onFadeIn += SlideOut;
    }

    void OnDestroy(){
        GameEvents.instance.onFadeOut -= SlideIn;
        GameEvents.instance.onFadeIn -= SlideOut;
}

    private void SlideIn(){
        if (!_isSlideIn){
            Vector3 targetPosition = ((RectTransform ) targetTransform).anchoredPosition;
            LeanTween.move(_rectTransform, targetPosition, moveDuration);
            _isSlideIn = true;
        }
    }

    private void SlideOut(){
        if (_isSlideIn){
            LeanTween.move(_rectTransform, _initPosition,  moveDuration);
            _isSlideIn = false;
        }
    }
}
