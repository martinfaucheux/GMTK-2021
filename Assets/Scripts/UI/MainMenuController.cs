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
    [SerializeField] float actionCoolDown = 0.2f;
    [SerializeField] bool isLevelNumberOnNewLine = true;

    private int selectedLevelId
    {
        get { return _selectedLevelId; }
        set
        {
            _selectedLevelId = value;
            UpdateUI();
        }
    }
    private int _selectedLevelId = 1;

    private float _lastActionTime;

    private bool canTakeAction
    {
        get
        {
            return Time.time - _lastActionTime > actionCoolDown;
        }
    }

    void Start()
    {
        SelectLastLevelPlayed();
    }

    void Update()
    {
        if (canTakeAction)
        {
            int vertValue = (int)(Input.GetAxisRaw("Horizontal"));
            bool _actionTaken = true;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                LoadSelectedLevel();
            }
            else if (vertValue < 0 && LevelLoader.instance.IsLevelUnlocked(_selectedLevelId - 1))
            {
                SelectPreviousLevel();
            }
            else if (vertValue > 0 && LevelLoader.instance.IsLevelUnlocked(_selectedLevelId + 1))
            {
                SelectNextLevel();
            }
            else
            {
                _actionTaken = false;
            }

            if (_actionTaken)
            {
                _lastActionTime = Time.time;
            }
        }
    }

    public void LoadSelectedLevel()
    {
        LevelLoader.instance.LoadLevel(selectedLevelId);
    }

    public void SelectLastLevelPlayed()
    {
        selectedLevelId = ProgressionManager.instance.lastPlayedLevelId;
    }

    public void SelectNextLevel()
    {
        if (LevelLoader.instance.IsLevelUnlocked(selectedLevelId + 1))
        {
            selectedLevelId++;
        }
    }

    public void SelectPreviousLevel()
    {
        if (selectedLevelId > 1)
            selectedLevelId--;
    }

    private void UpdateUI()
    {
        leftArrowButton.interactable = selectedLevelId > 1;
        rightArrowButton.interactable = LevelLoader.instance.IsLevelUnlocked(selectedLevelId + 1);
        string lineBreak = isLevelNumberOnNewLine ? "\n" : " ";
        levelText.SetText("Level" + lineBreak + selectedLevelId.ToString());
    }



}
