using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : SingletonBase<ProgressionManager>
{

    public int maxLevelId { get; private set; }
    public int lastPlayedLevelId { get; private set; }

    void Start()
    {
        RetrieveGameState();
    }

    public void SaveLastPlayedLevel(int lastPlayedLevelId)
    {
        this.lastPlayedLevelId = lastPlayedLevelId;
        SaveData();
    }

    public void SaveMaxLevel(int maxLevelId)
    {
        this.maxLevelId = maxLevelId;
        SaveData();
    }

    private void RetrieveGameState()
    {
        PlayerData playerData = DataSaver.LoadGameState();

        if (playerData != null)
        {
            maxLevelId = Mathf.Max(playerData.maxLevelId, 1);
            lastPlayedLevelId = Mathf.Max(playerData.currentLevelId, 1);
        }
        else
        {
            maxLevelId = 1;
            lastPlayedLevelId = 1;
        }
    }

    private void SaveData() => DataSaver.SaveGameState(maxLevelId, lastPlayedLevelId);

}
