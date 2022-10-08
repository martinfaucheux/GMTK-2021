using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerAnimator : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Animator _animator;
    [SerializeField] Color _frozenColor;
    [SerializeField] Color _rottenColor;
    [SerializeField] ParticleSystem _ashParticles;
    private float _colorChangeTransition
    {
        set { _spriteRenderer.material.SetFloat(materialColorTransitionProperty, value); }
    }
    private float _meltTransition
    {
        set { _spriteRenderer.material.SetFloat(materialRotTransitionProperty, value); }
    }
    private Color _shiftColor
    {
        set { _spriteRenderer.material.SetColor(materialShiftColorProperty, value); }
    }
    private static string materialColorTransitionProperty = "_Transition";
    private static string materialShiftColorProperty = "_ShiftColor";
    private static string materialRotTransitionProperty = "_Melt";
    private static float _colorTransitionDuration = 0.2f;
    private static float _fadeAwayDuration = 3f;
    private static float _meltMaxValue = 0.2f;
    public void SetFrozen()
    {
        _shiftColor = _frozenColor;
        _colorChangeTransition = 1f;
        _animator.SetBool("frozen", true);
    }

    public void SetRotten()
    {
        _shiftColor = _rottenColor;
        LeanTween.value(
            gameObject,
            t => _colorChangeTransition = t,
            0f,
            1f,
            _colorTransitionDuration
        );
        LeanTween.value(
            gameObject,
            t => _meltTransition = t,
            0f,
            _meltMaxValue,
            _colorTransitionDuration
        );

        _animator.SetBool("frozen", true);
    }

    public void SetUnFrozen()
    {
        // The burger juste got unfrozen
        LeanTween.value(
            gameObject,
            t => _colorChangeTransition = t,
            1f,
            0f,
            _colorTransitionDuration
        );
        _animator.SetBool("frozen", false);
    }

    public void SetBurned()
    {
        _animator.SetTrigger("burn");
        // Vector3 targetSize = transform.localScale;
        // targetSize.y = 0f;
        // targetSize.x *= 1.3f;
        // LeanTween.scale(gameObject, targetSize, _colorTransitionDuration + _fadeAwayDuration);

        LTSeq sequence = LeanTween.sequence();
        sequence.append(
            LeanTween.color(gameObject, Color.black, _colorTransitionDuration)
        );
        sequence.append(2f);
        sequence.append(() => _ashParticles?.Play());
        sequence.append(
            LeanTween.alpha(gameObject, 0f, _fadeAwayDuration).setEaseInQuint()
        );
    }
}
