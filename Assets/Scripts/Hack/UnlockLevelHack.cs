using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevelHack : MonoBehaviour
{


    public void UnlockAllLevels()
    {
        LevelLoader.instance.UnlockAllLevels();
        LevelLoader.instance.LoadFirstScene();
    }

    public void ResetGameState()
    {
        LevelLoader.instance.DeleteSavedData();
        LevelLoader.instance.LoadFirstScene();
    }


}
