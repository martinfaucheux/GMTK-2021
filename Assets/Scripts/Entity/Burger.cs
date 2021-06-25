using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burger : Entity
{
    public static List<Burger> burgerList;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;

    private bool _isBurned = false; 
    private bool _isEaten= false; 

    void Awake(){
        if(burgerList == null){
            burgerList = new List<Burger>{this};
        }
        else{
            burgerList.Add(this);
        }
    }


    protected override void Start(){
        base.Start();
    }

    public override void PreInteract(Entity entity){
        if (!_isBurned && !_isEaten){
            base.PreInteract(entity);
            _isEaten = true;
            burgerList.Remove(this);
        }
    }

    public override void Interact(Entity entity){
        if (_isEaten){
            base.Interact(entity);
            GameManager.instance.CheckWinCondition();
            Destroy(gameObject);
        }
    }

    void OnDestroy(){
        if(burgerList.Contains(this))
            burgerList.Remove(this);
    }

    public static void DisableAll(){
        foreach(Burger burger in burgerList){
            if (!burger._isEaten){
                burger._isBurned = true;
                burger.isBlocking = true;
                burger.playSound = false;
            }
        }
    }
    public static void PlayBurnAnimation(){
        foreach(Burger burger in burgerList){
            if(burger._isBurned){
                LeanTween.color(
                    burger.gameObject,
                    new Color(0f, 0f, 0f, 1f),
                    0.2f
                );
                burger.animator.SetTrigger("burn");
            }
        }
    }

    public override int GetResolveOrder() => 10;
}
