using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerTransition : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    private Vector2 _initPosition;
    private bool _isSlideIn = false;

    private float moveDuration{
        get{ return LevelLoader.instance.transitionDuration / 2;}
    }

    void Start()
    {
        _initPosition = ((RectTransform) transform).position;

        GameEvents.instance.onFadeOut += SlideIn;
        GameEvents.instance.onFadeIn += SlideOut;
    }

    void OnDestroy(){
        GameEvents.instance.onFadeOut -= SlideIn;
        GameEvents.instance.onFadeIn -= SlideOut;
}

    private void SlideIn(){
        if (!_isSlideIn){
            LeanTween.move(gameObject, targetTransform, moveDuration);
            _isSlideIn = true;
        }
    }

    private void SlideOut(){
        if (_isSlideIn){
            LeanTween.move(gameObject, _initPosition,  moveDuration);
            _isSlideIn = false;
        }
    }
}
