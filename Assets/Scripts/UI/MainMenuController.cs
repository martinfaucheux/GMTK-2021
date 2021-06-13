using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Button leftArrowButton;
    [SerializeField] Button rightArrowButton;
    [SerializeField] TextMeshProUGUI levelText;

    private int selectedLevelId {
        get{return _selectedLevelId;}
        set{
            _selectedLevelId = value;
            UpdateUI();
        }
    }
    private int _selectedLevelId = 1;

    void Start(){
        SelectLastLevelPlayed();
    }

    public void LoadSelectedLevel(){
        LevelLoader.instance.LoadLevel(selectedLevelId);
    }

    public void SelectLastLevelPlayed(){
        // TODO: last level
        selectedLevelId = LevelLoader.instance.maxLevelId;
    }

    public void SelectNextLevel(){
        if (LevelLoader.instance.IsLevelUnlocked(selectedLevelId + 1)){
            selectedLevelId++;
        }
    }

    public void SelectPreviousLevel(){
        if(selectedLevelId > 1)
            selectedLevelId --;
    }
    
    private void UpdateUI(){
        leftArrowButton.interactable = selectedLevelId > 1;
        rightArrowButton.interactable = LevelLoader.instance.IsLevelUnlocked(selectedLevelId + 1);
        levelText.SetText("Level\n" + selectedLevelId.ToString());
    }



}
