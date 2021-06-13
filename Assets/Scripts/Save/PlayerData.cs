using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // maximum level id that was reached
    public int maxLevelId;

    // current active level
    public int  currentLevelId;

    public Dictionary<int, bool> unlockedLevels;


    public PlayerData(Dictionary<int, bool> unlockedLevels, int maxLevelId, int currentLevelId){
        this.unlockedLevels = unlockedLevels;
        this.maxLevelId = maxLevelId;
        this.currentLevelId = currentLevelId;
    }

    public PlayerData(Dictionary<int, bool> unlockedLevels, int currentLevelId){
        this.unlockedLevels = unlockedLevels;
        this.currentLevelId = currentLevelId;

        // set max level
        foreach(KeyValuePair<int, bool> entry in unlockedLevels){           
            if(entry.Value & entry.Key > this.maxLevelId){
                this.maxLevelId = entry.Key;
            }
        }
    }


    public PlayerData(int maxLevelId, int currentLevelId) => new PlayerData(
        new Dictionary<int, bool>(),
        maxLevelId,
        currentLevelId
    );

    public PlayerData(LevelLoader levelLoader) => new PlayerData(
            levelLoader.maxLevelId,
            levelLoader.currentLevelId
    );

}
