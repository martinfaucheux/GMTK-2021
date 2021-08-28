using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicToggle : MonoBehaviour
{
    [SerializeField] Image crossImage;
    [SerializeField] TextMeshProUGUI onOffText;
    [SerializeField] string soundName = "Game Music";


    private bool _isEnabled = true;

    private Sound music
    {
        get { return AudioManager.instance.Get(soundName); }
    }


    public void Toggle()
    {
        if (_isEnabled)
        {
            Disable();
        }
        else
        {
            Enable();
        }
    }

    private void Enable()
    {
        music.Unmute();
        SetText("On");
        crossImage.enabled = false;
        _isEnabled = true;
    }

    private void Disable()
    {
        music.Mute();
        SetText("Off");
        crossImage.enabled = true;
        _isEnabled = false;
    }

    private void SetText(string text)
    {
        onOffText?.SetText(text);
    }
}
