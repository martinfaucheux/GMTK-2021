using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamBubble : MonoBehaviour
{

    public float minScale = 0.5f;
    public float maxScale = 1f;
    public float lifeDuration = 2f;
    public float lifeDurationVariation = 0.3f;

    public SpriteRenderer spriteRenderer;

    public void Animate()
    {
        Reset();

        float duration = lifeDuration + lifeDurationVariation * Random.Range(-1f, 1f);

        LeanTween.scale(
            gameObject,
            Random.Range(minScale, maxScale) * Vector3.one,
            duration
        ).setLoopPingPong(1).setOnComplete(TweenCallback);

        LeanTween.alpha(spriteRenderer.gameObject, 0f, duration * 2f).setEaseInQuint();
    }

    private void Reset()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.zero;
        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
    }

    private void TweenCallback()
    {
        LeanTween.cancel(gameObject);
        gameObject.SetActive(false);
    }

}
