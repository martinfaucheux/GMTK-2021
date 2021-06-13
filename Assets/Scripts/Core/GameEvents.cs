using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance = null;
    public event Action onWin;
    public event Action onFadeOut;
    public event Action onFadeIn;

    public event Action<int> onBlobCollision;

    //Awake is always called before any Start functions
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


    public void WinTrigger()
    {
        if (onWin != null)
        {
            onWin();
        }
    }

    public void FadeOutTrigger()
    {
        if (onFadeOut != null)
        {
            onFadeOut();
        }
    }

    public void FadeInTrigger()
    {
        if (onFadeIn != null)
        {
            onFadeIn();
        }
    }

    public void BlobCollisionTrigger(int gameObjectId)
    {
        if (onBlobCollision != null)
        {
            onBlobCollision(gameObjectId);
        }
    }

}
