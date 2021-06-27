using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorController : MonoBehaviour
{
    [SerializeField] Color seaSunsetColor;
    [SerializeField] float sunsetColorTemperature = 50f;
    [SerializeField] float transitionDuration = 3f;
    [SerializeField] int levelTrigger = 25;
    [SerializeField] GameObject seaGameObject;
    [SerializeField] Volume _volumeComponent;

    private WhiteBalance _whiteBalance;

    private bool _hasChanged = false;

    private float _temperatureValue
    {
        get { return _whiteBalance.temperature.value;}
        set { _whiteBalance.temperature.value = value;}
    }

    void Start(){
        VolumeProfile _volumeProfile = _volumeComponent.profile;

        if (!_volumeProfile.TryGet<WhiteBalance>(out _whiteBalance))
        {
            _whiteBalance = _volumeProfile.Add<WhiteBalance>(true);
        }
        _temperatureValue = 0f;

        if(LevelLoader.instance.currentLevelId > levelTrigger){
            InstantTrigger();
        }
    }

    private void InstantTrigger(){
        if(!_hasChanged){
            seaGameObject.GetComponent<SpriteRenderer>().color = seaSunsetColor;
            _temperatureValue = sunsetColorTemperature;
            _hasChanged = true;
        }
    }


    private void SmoothTrigger()
    {
        if(!_hasChanged){
            SwitchSeaColor(seaSunsetColor);
            SetTemperature(sunsetColorTemperature);
            _hasChanged = true;
        }
    }

    public void SwitchSeaColor(Color color){
        LeanTween.color(seaGameObject, color, transitionDuration);
    }

    public void SetTemperature(float targetTemperature){
        StartCoroutine(SetTemperatureCoroutine(targetTemperature));
    }

    private IEnumerator SetTemperatureCoroutine(float targetTemperature){
        Debug.Log("Update Temperature");
        float timeSinceStart = 0f;
        float initTemerature = _temperatureValue;

        while(timeSinceStart < transitionDuration){
            float newTemperature = initTemerature + (targetTemperature - initTemerature) * (timeSinceStart / transitionDuration);
            _temperatureValue = newTemperature;
            timeSinceStart += Time.deltaTime;
            yield return null;
        }
        _temperatureValue = targetTemperature;
    }
}
