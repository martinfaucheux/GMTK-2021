using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsructionHand : MonoBehaviour
{


    [SerializeField] float movementAmplitude = 1f;
    [SerializeField] float timeBeforeShow = 3f;
    [SerializeField] float transitionDuration = 1f;
    [SerializeField] float pauseDuration = 1f;

    [SerializeField] Vector2[] directions;

    SpriteRenderer _spriteRenderer;

    int _directionIndex = 0;

    float _lastActionTime;

    Vector3 _initPos;


    float nextActionTime
    {
        get { return _lastActionTime + transitionDuration + pauseDuration; }
    }

    private void Start()
    {
        _initPos = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _lastActionTime = Time.time;
        SetOpacity(0);

        PlayerController.OnGetCommand += Hide;

    }

    private void OnDestroy()
    {
        PlayerController.OnGetCommand -= Hide;
    }

    private void Update()
    {
        if (Time.time > nextActionTime)
        {
            Vector2 direction = directions[_directionIndex];
            _directionIndex = (_directionIndex + 1) % directions.Length;
            _lastActionTime = Time.time;
            MoveHand(direction);
        }
    }

    private void MoveHand(Vector2 direction)
    {

        transform.position = _initPos;
        SetOpacity(1);

        // move the hand
        Vector3 targetPosition = _initPos + movementAmplitude * (Vector3)direction;
        LeanTween.move(gameObject, targetPosition, transitionDuration).setEaseOutCubic();

        // change opacity
        LeanTween.alpha(gameObject, 0f, pauseDuration / 3).setDelay(transitionDuration).setEaseInCubic();
    }

    private void SetOpacity(float oppacity)
    {
        Color _color = _spriteRenderer.color;
        _color.a = oppacity;
        _spriteRenderer.color = _color;
    }

    private void Hide(Direction direction)
    {
        gameObject.SetActive(false);
    }


}
