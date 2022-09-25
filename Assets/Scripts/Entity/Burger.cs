using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burger : Entity
{
    public static List<Burger> burgerList;

    [field: SerializeField]
    public bool isFrozen { get; private set; }
    [SerializeField] BurgerAnimator _burgerAnimator;
    private bool _isBurned = false;
    private bool _isEaten = false;

    public string frozenSoundName = "Glass";
    private string _normalSoundName;

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
            collidingSoundName = frozenSoundName;
            _burgerAnimator.SetFrozen();
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
        // TODO: execute this with end of turn event
        Color blackColor = new Color(0f, 0f, 0f, 1f);
        foreach (Burger burger in burgerList)
        {
            if (!burger._isEaten)
            {
                if (burger._isBurned)
                {
                    // The burger juste got burned
                    burger._burgerAnimator.SetBurned();
                }
                else
                {
                    // The burger juste got unfrozen
                    burger._burgerAnimator.SetUnFrozen();
                }
            }
        }
    }

    public override int GetResolveOrder() => 10;
}
