using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burger : Entity
{
    public static List<Burger> burgerList;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;

    [field: SerializeField]
    public bool isFrozen { get; private set; }
    private bool _isBurned = false;
    private bool _isEaten = false;
    private float frozenTransition
    {
        set { spriteRenderer.material.SetFloat(materialTransitionProperty, value); }
    }
    public string frozenSoundName = "Glass";
    private string _normalSoundName;
    private static string materialTransitionProperty = "_Transition";
    private static float _transitionTime = 0.2f;

    void Awake()
    {
        if (burgerList == null)
            burgerList = new List<Burger> { this };
        else
            burgerList.Add(this);
    }


    protected override void Start()
    {
        base.Start();
        _normalSoundName = collidingSoundName;

        if (isFrozen)
        {
            isBlocking = true;
            frozenTransition = 1f;
            animator.SetBool("frozen", true);
            collidingSoundName = frozenSoundName;
        }
    }

    public override void PreInteract(Entity entity)
    {
        if (!_isBurned && !_isEaten)
        {
            base.PreInteract(entity);
            _isEaten = true;
            burgerList.Remove(this);
            ((Guy)entity).blob.Amaze();
            // remove matrix collider to prevent other collision within the same turn
            matrixCollider.Unregister();
        }
    }

    public override void Interact(Entity entity)
    {
        // TODO: change "out of reach" behavior to allow playing sound when collision and frozen
        if (_isEaten)
        {
            base.Interact(entity);
            GameManager.instance.CheckWinCondition();
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (burgerList.Contains(this))
            burgerList.Remove(this);
    }

    public static void ResolveExplosion()
    {
        foreach (Burger burger in burgerList)
        {
            if (!burger._isEaten)
            {
                if (burger.isFrozen)
                {
                    burger.isFrozen = false;
                    burger.isBlocking = false;
                    burger.collidingSoundName = burger._normalSoundName;
                }
                else
                {
                    burger._isBurned = true;
                    burger.isBlocking = true;
                    burger.playSound = false;
                }
            }
        }
    }


    public static void AnimateExplosion()
    {
        Color blackColor = new Color(0f, 0f, 0f, 1f);
        foreach (Burger burger in burgerList)
        {
            if (!burger._isEaten)
            {
                if (burger._isBurned)
                {
                    // The burger juste got burned
                    LeanTween.color(burger.gameObject, blackColor, _transitionTime);
                    burger.animator.SetTrigger("burn");
                }
                else
                {
                    // The burger juste got unfrozen
                    LeanTween.value(
                        burger.gameObject,
                        t => burger.frozenTransition = t,
                        1f,
                        0f,
                        _transitionTime
                    );
                    burger.animator.SetBool("frozen", false);
                }
            }
        }
    }

    public override int GetResolveOrder() => 10;
}
