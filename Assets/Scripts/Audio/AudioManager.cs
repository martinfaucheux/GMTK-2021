
using System.Collections.Generic; 
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] bool keepThroughScenes;
    [SerializeField] Sound[] sounds;

    private Dictionary<string, Sound> _soundDict;
    void Awake()
    {
        CheckSingleton();

        if (keepThroughScenes)
            DontDestroyOnLoad(gameObject);
        
        InitializeAudioSources();
    }

    private void InitializeAudioSources(){
        _soundDict = new Dictionary<string, Sound>();
        foreach(Sound sound in sounds){
            sound.source = gameObject.AddComponent<AudioSource>();
            _soundDict.Add(sound.name, sound);
        }
    }

    public void Play(string soundName){
        if(_soundDict.ContainsKey(soundName)){
            _soundDict[soundName].Play();
        }
        else{
            Debug.LogError("Unknown sound: " + soundName);
        }
    }

    public void Stop(string soundName){
        if(_soundDict.ContainsKey(soundName)){
            _soundDict[soundName].Stop();
        }
        else{
            Debug.LogError("Unknown sound: " + soundName);
        }
    }

    private void CheckSingleton(){
        if(instance == null){
            instance = this;
        }
        else{
            Debug.Log("Audio manager found, destroying.");
            Destroy(gameObject);
            return;
        }
    }

    void OnDestroy(){
        foreach(KeyValuePair<string, Sound> entry in _soundDict){           
            Sound sound = entry.Value;
            Destroy(sound.source);
        }    
    }
}
