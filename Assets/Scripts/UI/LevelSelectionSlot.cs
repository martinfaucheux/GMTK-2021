using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectionSlot : MonoBehaviour
{
    [SerializeField] int levelId;
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] Button button;
    [SerializeField] bool alwaysUnlocked;

    void Start(){
        textComponent.SetText(levelId.ToString());
        SetInteractable();
        // Debug.Log("Level " + levelId.ToString() + ": " + LevelLoader.instance.IsLevelUnlocked(levelId));
    }
    
    public void Load(){
        LevelLoader.instance.LoadLevel(levelId);
    }

    private void SetInteractable(){
        button.interactable = LevelLoader.instance.IsLevelUnlocked(levelId) || alwaysUnlocked;
    }

}
