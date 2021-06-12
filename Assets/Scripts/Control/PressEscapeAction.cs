using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressEscapeAction : MonoBehaviour
{
    [SerializeField] UnityEvent onEscapePressedEvent;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            onEscapePressedEvent.Invoke();
        }       
    }
}
