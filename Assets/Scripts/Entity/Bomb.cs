using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Entity
{
    [SerializeField] GameObject explosionPrefab;
    static HashSet<Blob> _explosionCause;
    protected override void Start()
    {
        base.Start();
        GameEvents.instance.onStartOfTurn += CleanCauses;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameEvents.instance.onStartOfTurn -= CleanCauses;
    }


    public override void PreInteract(Entity entity)
    {
        base.PreInteract(entity);
        AddToCauses(entity);
        Burger.ResolveExplosion();
    }

    private static void AddToCauses(Entity entity)
    {
        // For a given turn, keep in memory which blob caused an explosion
        // if this blob should eat a burger, it should not be prevented
        Guy guy = entity as Guy;
        if (guy != null)
        {
            if (guy.blob != null)
                _explosionCause.Add(guy.blob);
        }
    }

    public static bool IsExplosionCause(Entity entity)
    {
        Guy guy = entity as Guy;
        if (guy != null)
        {
            if (guy.blob != null)
                return _explosionCause.Contains(guy.blob);
        }
        return false;
    }



    public override void Interact(Entity entity)
    {
        base.Interact(entity);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Burger.AnimateExplosion();
        CameraShake.instance?.Shake();
        Destroy(gameObject);
    }

    private static void CleanCauses() => _explosionCause = new HashSet<Blob>();
}
