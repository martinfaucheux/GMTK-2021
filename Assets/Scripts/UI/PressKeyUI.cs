using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressKeyUI : MonoBehaviour
{


    [SerializeField] float spriteSwapPeriod = 0.4f;
    [SerializeField] Sprite pressedSprite;
    [SerializeField] Sprite releasedSprite;


    // [SerializeField] float spritePressedScale = 0.5f;
    [SerializeField] float timeBeforeShow = 1f;

    [SerializeField] Image imageComponent;

    private bool _isAnimPlaying = false;

    private float _lastSwapTime = 0f;

    private bool _keyPressedSprite = false;


    void Start()
    {
        imageComponent.enabled = false;
        GameEvents.instance.onWin += PlayAnim;
    }

    void OnDestroy(){
        GameEvents.instance.onWin -= PlayAnim;
    }

    void Update()
    {
        if(_isAnimPlaying){
            if(Time.time - _lastSwapTime > spriteSwapPeriod){
                SwapSprite();
                _lastSwapTime = Time.time;
            }
        }
    }

    private void SwapSprite(){
        imageComponent.sprite =  _keyPressedSprite ? releasedSprite : pressedSprite;
        _keyPressedSprite = !_keyPressedSprite;
    }

    private void PlayAnim(){
        StartCoroutine(PlayAnimCoroutine());
    }

    private IEnumerator PlayAnimCoroutine(){
        yield return new WaitForSeconds(timeBeforeShow);
        imageComponent.enabled = true;
        _lastSwapTime = Time.time;
        _isAnimPlaying = true;
    }
}
