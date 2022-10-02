using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class LongPressEventTrigger : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Tooltip("How long must pointer be down on this object to trigger a long press")]
    public float durationThreshold = 1.0f;

    public UnityEvent onLongPress = new UnityEvent();

    private bool _isPointerDown = false;
    private bool _longPressTriggered = false;
    private float _timePressStarted;


    private void Update()
    {
        if (_isPointerDown && !_longPressTriggered)
        {
            if (Time.time - _timePressStarted > durationThreshold)
            {
                _longPressTriggered = true;
                onLongPress.Invoke();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _timePressStarted = Time.time;
        _isPointerDown = true;
        _longPressTriggered = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerDown = false;
    }
}