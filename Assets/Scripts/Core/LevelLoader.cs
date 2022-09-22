using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : SingletonBase<LevelLoader>
{
    public Dictionary<int, bool> unlockedLevels = new Dictionary<int, bool>();
    public int currentLevelId;
    public float transitionDuration = 0.5f;
    public List<int> unlockedOnWin;
    private bool _isMainMenu = false;

    void Start()
    {
        currentLevelId = SceneManager.GetActiveScene().buildIndex;
        _isMainMenu = (currentLevelId == 0);

        // trigger fade in at start
        GameEvents.instance.FadeInTrigger();
    }

    public void LoadNextLevel()
    {
        if (currentLevelId < SceneManager.sceneCountInBuildSettings)
        {
            int nextLevelId = currentLevelId + 1;
            UnlockLevel(nextLevelId);

            LoadLevel(nextLevelId);
        }
    }

    public void LoadPreviousLevel()
    {
        if (currentLevelId > 0)
            LoadLevel(currentLevelId - 1);
    }

    public void ReloadLevel() => LoadLevel(currentLevelId);


    public void LoadLastLevelPlayed()
    {
        int lastPlayedLevelId = ProgressionManager.instance.lastPlayedLevelId;
        LoadLevel(lastPlayedLevelId, false);
    }

    public void LoadFirstScene()
    {
        Destroy(AudioManager.instance); // destroy so main menu music will play
        LoadLevel(0, false);
    }

    public void LoadLastScene(bool saveProgress)
    {
        LoadLevel(SceneManager.sceneCountInBuildSettings - 1, saveProgress);
    }

    public void LoadLevel(int levelID, bool doSaveData = true)
    {
        GameEvents.instance.FadeOutTrigger();

        if (doSaveData)
            ProgressionManager.instance.SaveLastPlayedLevel(levelID);

        StartCoroutine(DelayLoadScene(levelID, transitionDuration));
    }

    public bool IsLevelUnlocked(int levelId) => levelId <= ProgressionManager.instance.maxLevelId;

    public bool IsPreviousLevelAvailable() => currentLevelId > 1;

    public bool IsNextLevelAvailable() => (currentLevelId < ProgressionManager.instance.maxLevelId);

    private void UnlockLevel(int levelID)
    {
        bool hasChanged = !IsLevelUnlocked(levelID);
        unlockedLevels[levelID] = true;

        if (hasChanged)
        {
            if (ProgressionManager.instance.maxLevelId < levelID)
                ProgressionManager.instance.SaveMaxLevel(levelID);
        }
    }

    public void UnlockNextLevels()
    {
        foreach (int newLevelId in unlockedOnWin)
            UnlockLevel(newLevelId);
    }


    public void DeleteSavedData() => DataSaver.DeleteSavedData();

    private IEnumerator DelayLoadScene(int sceneBuildIndex, float seconds)
    {
        // use WaitForSecondsRealtime to allow moving when timeScale = 0 (game paused)
        yield return new WaitForSecondsRealtime(seconds);
        SceneManager.LoadScene(sceneBuildIndex);
    }


    public void Quit() => Application.Quit();


    public static string LevelDictToString(Dictionary<int, bool> dict)
    {
        string res = "";
        foreach (KeyValuePair<int, bool> entry in dict)
            res += entry.Key.ToString() + ": " + entry.Value.ToString();
        return res;
    }
}
