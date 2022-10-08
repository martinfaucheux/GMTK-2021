using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public MatrixCollider matrixCollider { get; private set; }
    public bool isStopMovement = false; // wheter displacement is allowed on the case but it can't go further
    public bool interactWhenOutOfReach = false;
    public bool canBeBlocked = true;
    public string collidingSoundName;

    public bool playSound = true;

    public Vector2Int matrixPosition
    {
        get { return matrixCollider.matrixPosition; }
    }

    protected virtual void Start()
    {
        matrixCollider = GetComponent<MatrixCollider>();
        GameEvents.instance.onEndOfTurn += OnEndOfTurn;
    }

    protected virtual void OnDestroy()
    {
        GameEvents.instance.onEndOfTurn -= OnEndOfTurn;
    }

    public virtual void PreInteract(Entity collidingEntity)
    {
        // called when forecasting movements, before the entities are visally moved
    }

    public virtual void Interact(Entity collidingEntity)
    {
        // called after the entities are visally moved
        PlaySound();
    }

    private void PlaySound()
    {
        if (playSound && collidingSoundName != "")
            AudioManager.instance?.Play(collidingSoundName);

    }

    public virtual bool CanInteract(Entity otherEntity) => true;

    public virtual bool IsBlocking(Entity otherEntity) => false;

    public virtual int GetResolveOrder() => 0;

    protected virtual void OnEndOfTurn() { }
}
