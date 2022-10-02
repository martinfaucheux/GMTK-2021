using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class MultiPressEventTrigger : UIBehaviour, IPointerDownHandler
{
    [Tooltip("How many time must the pointer be pressed on this object to trigger a mutlti press")]
    [SerializeField] int pressNumber = 7;

    [Tooltip("Maximum time between 2 press to be considered as multi press")]
    [SerializeField] float _interPressMaxDuration = 0.7f;

    [SerializeField] UnityEvent onLongPress = new UnityEvent();

    private int _pressCount;
    private float _lastPressedTime;


    public void OnPointerDown(PointerEventData eventData)
    {

        if (Time.time < _lastPressedTime + _interPressMaxDuration)
        {
            _pressCount++;
            if (_pressCount >= pressNumber)
            {
                _pressCount = 0;
                onLongPress.Invoke();
            }
        }
        else
        {
            _pressCount = 1;
        }
        _lastPressedTime = Time.time;
    }
}