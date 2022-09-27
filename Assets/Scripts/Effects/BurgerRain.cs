using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerRain : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;

    void Start()
    {
        GameEvents.instance.onWin += TriggerParticles;
    }
    void OnDestroy()
    {
        GameEvents.instance.onWin -= TriggerParticles;

    }
    private void TriggerParticles() => _particleSystem.Play();

}
