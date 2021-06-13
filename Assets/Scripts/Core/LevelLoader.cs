using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance = null;

    // maximum level that was reached
    public int maxLevelId;
    // current loaded level id

    public Dictionary<int, bool> unlockedLevels = new Dictionary<int, bool>();
    public int currentLevelId;

    public float transitionDuration = 0.5f;

    [SerializeField] bool loadSaveData = false;

    // cache last level that was played (for play button)
    private int _lastLevelPlayedID = 1;
    private bool _isMainMenu = false;

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

    // Start is called before the first frame update
    void Start()
    {
        currentLevelId = SceneManager.GetActiveScene().buildIndex;
        _isMainMenu = (currentLevelId == 0);

        // trigger fade in at start
        GameEvents.instance.FadeInTrigger();

        if (loadSaveData){
            RetrieveGameState();
        }
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
        {
            LoadLevel(currentLevelId - 1);
        }
    }

    public void ReloadLevel()
    {
        LoadLevel(currentLevelId);
    }

    public void LoadLastLevelPlayed(){
        LoadLevel(_lastLevelPlayedID, false);
    }

    public void LoadFirstScene(){
        LoadLevel(0, false);
    }

    private void LoadLevel(int levelID, bool doSaveData = true){

        GameEvents.instance.FadeOutTrigger();

        if (doSaveData){
            SaveData(unlockedLevels, currentLevelID: levelID);
        }
        
        _lastLevelPlayedID = levelID;
        StartCoroutine(DelayLoadScene(levelID, transitionDuration));
    }

    public bool IsLevelUnlocked(int levelId){
        return (unlockedLevels.ContainsKey(levelId) && unlockedLevels[levelId]); 
    }

    public bool IsPreviousLevelAvailable(){
        return currentLevelId > 1;
    }

    public bool IsNextLevelAvailable(){
        return (currentLevelId < maxLevelId);
    }

    private void UnlockLevel(int levelID){
        
        bool hasChanged = !IsLevelUnlocked(levelID);

        unlockedLevels[levelID] = true;

        if (hasChanged){
            if (maxLevelId < levelID){
                maxLevelId = levelID;
            }
            // TODO: add dict
            SaveData(unlockedLevels, maxLevelId: levelID);
        }
    }

    public void SaveData(Dictionary<int, bool> unlockedLevels, int maxLevelId = -1, int currentLevelID = -1){
        if (maxLevelId < 0 ){
            maxLevelId = this.maxLevelId;
        }

        if (currentLevelID < 0){
            currentLevelID = this.currentLevelId;
        }
        DataSaver.SaveGameState(unlockedLevels, maxLevelId, currentLevelID);

    }

    public void RetrieveGameState(){
        PlayerData playerData = DataSaver.LoadGameState();

        if (playerData != null){
            maxLevelId = playerData.maxLevelId;
            _lastLevelPlayedID = playerData.currentLevelId;
            unlockedLevels = playerData.unlockedLevels;
        }
    }

    public void DeleteSavedData(){
        DataSaver.DeleteSavedData();
        maxLevelId = 1;
        _lastLevelPlayedID = 1;
    }

    private IEnumerator DelayLoadScene(int sceneBuildIndex, float seconds)
    {
        // use WaitForSecondsRealtime to allow moving when timeScale = 0 (game paused)
        yield return new WaitForSecondsRealtime(seconds);
        SceneManager.LoadScene(sceneBuildIndex);
    }


    public void Quit(){
        Application.Quit();
    }
}
