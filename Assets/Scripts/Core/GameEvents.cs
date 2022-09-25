using System;
using UnityEngine;

public class GameEvents : SingletonBase<GameEvents>
{
    public event Action onWin;
    public event Action onFadeOut;
    public event Action onFadeIn;
    public event Action onStartOfTurn;
    public event Action onEndOfTurn;
    public event Action<int> onBlobCollision;

    public void WinTrigger()
    {
        if (onWin != null)
            onWin();
    }

    public void FadeOutTrigger()
    {
        if (onFadeOut != null)
            onFadeOut();
    }

    public void FadeInTrigger()
    {
        if (onFadeIn != null)
            onFadeIn();
    }

    public void BlobCollisionTrigger(int gameObjectId)
    {
        if (onBlobCollision != null)
            onBlobCollision(gameObjectId);
    }
    public void StartOfTurnTrigger()
    {
        if (onStartOfTurn != null)
            onStartOfTurn();
    }
    public void EndOfTurnTrigger()
    {
        if (onEndOfTurn != null)
            onEndOfTurn();
    }
}
