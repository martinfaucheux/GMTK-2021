using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimation : MonoBehaviour
{
    [SerializeField] RectTransform eyebrowsRectTransform;
    [SerializeField] float animDuration;
    [SerializeField] float animAmplitude;
    [SerializeField] float animPeriod;
    
    private float _nextAnimTime;
    private Vector3 _initPos;
    private Vector3 _targetPos;

    void Start(){
        _initPos = eyebrowsRectTransform.position;
        _targetPos = _initPos + new Vector3(0f, animAmplitude, 0f);
        _nextAnimTime = Time.time + Random.Range(0f, animPeriod);
    }

    void Update(){
        if(Time.time > _nextAnimTime){
            Animate();
            SetNextAnimTime();
        }
    }

    private void Animate(){
        LeanTween.move(eyebrowsRectTransform, _targetPos, animDuration / 4).setLoopPingPong(2).setEaseInSine();
    }

    private void SetNextAnimTime(){
        _nextAnimTime = Time.time + animPeriod * Random.Range(0.9f, 1.1f);
    }
    
}
