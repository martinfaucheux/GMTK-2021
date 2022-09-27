using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAnimation : MonoBehaviour
{
    [SerializeField] Transform faceTransform;

    [SerializeField] float breathAmplitude;
    [SerializeField] float breathPeriod;

    [SerializeField] bool doBreath;
    [SerializeField] bool doBlink;
    [SerializeField] bool doMouthTremble;
    [SerializeField] float blinkPeriod;
    [SerializeField] float blinkDuration;
    [SerializeField] float wowDuration;
    [SerializeField] float wowAmplitude;
    [SerializeField] Sprite blinkEyeSprite;
    [SerializeField] Sprite wowMouthsprite;

    [SerializeField] SpriteRenderer eyesSpriteRenderer;
    [SerializeField] SpriteRenderer mouthSpriteRenderer;
    [SerializeField] Transform eyeBrowsTransform;
    private Sprite _initEyesSprite;
    private Sprite _initMouthSprite;

    private float _lastBlink;
    private bool _isBlinking = false;

    void Start()
    {

        _initMouthSprite = mouthSpriteRenderer.sprite;
        _initEyesSprite = eyesSpriteRenderer.sprite;
        _lastBlink = Time.time + Random.Range(0f, blinkPeriod);

        if (doBreath)
        {
            Vector3 targetPos = new Vector3(0f, breathAmplitude, 0f);
            LeanTween.moveLocal(
                faceTransform.gameObject,
                targetPos,
                breathPeriod
            ).setLoopPingPong().setDelay(Random.Range(0f, 2 * breathPeriod));
        }

        if (doMouthTremble)
        {
            Vector3 displacement = 0.03f * Vector3.right;
            mouthSpriteRenderer.transform.position -= 0.3f * displacement;
            LeanTween.moveLocal(
                mouthSpriteRenderer.gameObject,
                displacement,
                0.1f
            ).setLoopPingPong();
        }

        GameEvents.instance.onBlobCollision += Wow;
    }

    void OnDestroy()
    {
        GameEvents.instance.onBlobCollision -= Wow;
    }

    void Update()
    {
        if (doBlink)
        {
            if (_isBlinking)
            {
                if (Time.time - _lastBlink > blinkDuration)
                {
                    eyesSpriteRenderer.sprite = _initEyesSprite;
                    _isBlinking = false;
                }
            }
            else
            {
                if (Time.time - _lastBlink > blinkPeriod)
                {
                    eyesSpriteRenderer.sprite = blinkEyeSprite;
                    _isBlinking = true;
                    _lastBlink = Time.time;
                }
            }
        }
    }

    private void Wow(int gameObjectId)
    {
        if (gameObjectId == gameObject.GetInstanceID())
        {
            Wow();
        }
    }

    private void Wow()
    {
        mouthSpriteRenderer.sprite = wowMouthsprite;
        Vector3 targetPos = new Vector3(0f, wowAmplitude, 0f);
        LeanTween.moveLocal(eyeBrowsTransform.gameObject, targetPos, wowDuration).setLoopPingPong(1).setOnComplete(SetMouthNormal);
    }

    private void SetMouthNormal()
    {
        mouthSpriteRenderer.sprite = _initMouthSprite;
    }
}
