using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float actionDuration;
    
    public bool isWin = false;

    public bool playerCanMove{
        get{
            return !isWin;
        }
    }

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a CollisionMatrix.
            Destroy(gameObject);

    }

    void Update(){
        if(Input.anyKeyDown & isWin){
            LevelLoader.instance.LoadNextLevel();
        }
    }

    public void Win(){
        GameEvents.instance.WinTrigger();
        isWin = true;
        Debug.Log("YOU WIN");
        // LevelLoader.instance.LoadNextLevel();
    }
}
