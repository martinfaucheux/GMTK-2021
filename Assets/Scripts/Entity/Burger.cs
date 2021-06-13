using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burger : Entity
{
    public static List<Burger> burgerList;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;

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

    public override void Interact(Blob collidingBlob)
    {
        base.Interact(collidingBlob);
        burgerList.Remove(this);
        GameManager.instance.CheckWinCondition();
        Destroy(gameObject);
    }

    void OnDestroy(){
        if(burgerList.Contains(this))
            burgerList.Remove(this);
    }

    public static void DestroyAll(){
        foreach(Burger burger in burgerList){
            // Destroy(burger.gameObject);
            burger.Burn();
        }
    }

    private void Burn(){
         // paint it black black
        LeanTween.color(gameObject, new Color(0f, 0f, 0f, 1f), 0.2f);
        isInteractable = false;
        isBlocking = true;
        animator.SetTrigger("burn");
    }
}
