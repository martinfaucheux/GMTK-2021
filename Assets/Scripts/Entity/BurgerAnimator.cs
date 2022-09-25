using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerAnimator : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Animator _animator;
    private float _frozenTransition
    {
        set { _spriteRenderer.material.SetFloat(materialTransitionProperty, value); }
    }
    private static string materialTransitionProperty = "_Transition";
    private static float _transitionTime = 0.2f;

    public void SetFrozen()
    {
        _frozenTransition = 1f;
        _animator.SetBool("frozen", true);
    }

    public void SetUnFrozen()
    {
        // The burger juste got unfrozen
        LeanTween.value(
            gameObject,
            t => _frozenTransition = t,
            1f,
            0f,
            _transitionTime
        );
        _animator.SetBool("frozen", false);
    }

    public void SetBurned()
    {
        // The burger juste got burned
        LeanTween.color(gameObject, Color.black, _transitionTime);
        _animator.SetTrigger("burn");
    }
}
