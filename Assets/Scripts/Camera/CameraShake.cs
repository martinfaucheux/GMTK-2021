using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : SingletonBase<CameraShake>
{
    [SerializeField] float amplitude = 10f;
    [SerializeField] float duration = 0.5f;
    [SerializeField] float period = 0.2f;
    [SerializeField] float delay = 0f;
    private float _timeSinceStart = 0f;
    private bool _isShaking = false;
    private Vector3 _initPos;

    public void Shake()
    {
        if (!_isShaking)
            StartCoroutine(ShakeCoroutine());
    }

    public void ShakeOnce(Direction direction)
    {
        _isShaking = true;
        Vector3 offset = amplitude * 0.3f * (Vector2)direction.ToPos();
        LeanTween.move(
            gameObject,
            transform.position + offset,
            duration * 0.1f
        ).setLoopPingPong(1).setOnComplete(() => _isShaking = false);

    }

    private IEnumerator ShakeCoroutine()
    {
        _isShaking = true;
        float randomAngle = Random.Range(0f, 360f);
        _initPos = transform.position;

        yield return new WaitForSeconds(delay);
        _timeSinceStart = 0f;
        while (_timeSinceStart < duration)
        {
            transform.position = _initPos + GetDisplacement(_timeSinceStart / duration, randomAngle);
            _timeSinceStart += Time.deltaTime;
            yield return null;
        }
        transform.position = _initPos;
        _isShaking = false;
    }

    private Vector3 GetDisplacement(float t, float angle)
    {
        float amp = Mathf.Sin(t * Mathf.PI * 2f / period) * amplitude;
        return amp * (1 - t) * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
    }

}
