using UnityEngine.Audio;

using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(.1f, 3f)]
    public float pitch = 1f;
    [Range(0f, 1f)]
    public float randomizePitch = 0f;
    public bool loop;
    public bool playOnStart;

    public bool isEnabled = true;

    public AudioSource source{
        get => _source;
        set => SetSource(value);
    }

    private AudioSource _source;

    private void SetSource(AudioSource audioSource){
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        _source = audioSource;

        if (playOnStart){
            Play();
        }
    }

    public void Play(){

        if(randomizePitch > 0){
            source.pitch = pitch + randomizePitch * Random.Range(-1, 1);
        }

        source.Play();
    }

    public void Unmute(){
        Debug.Log("Unmute");
        isEnabled = true;
        _source.volume = volume;
    }

    public void Mute(){
        Debug.Log("Mute");
        isEnabled = false;
        _source.volume = 0f;
    }

    public void Stop() => _source.Stop();
}
