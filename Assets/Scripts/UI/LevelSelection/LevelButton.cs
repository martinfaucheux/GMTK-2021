using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{

    [SerializeField] Image _bgImage;
    [SerializeField] TextMeshProUGUI _textComponent;
    [SerializeField] Button _button;
    [SerializeField] Color _lockedColor;
    [SerializeField] Color _lockedTextColor;
    [SerializeField] Transform treeContainer;
    private int _sceneId;
    public void LoadLevel() => LevelLoader.instance.LoadLevel(_sceneId);

    public void InitializeButton(int levelId, LevelZone levelZone)
    {
        _sceneId = levelId;
        _textComponent.text = _sceneId.ToString();
        bool IsLevelUnlocked = LevelLoader.instance.IsLevelUnlocked(_sceneId);

        if (IsLevelUnlocked)
        {
            ChangeTreeMaterials(levelZone);
            _bgImage.color = levelZone.buttonColor;
            _textComponent.color = levelZone.buttonTextColor;
        }
        else
        {
            treeContainer.gameObject.SetActive(false);
            _bgImage.color = _lockedColor;
            _textComponent.color = _lockedTextColor;
            _button.interactable = false;
        }
    }

    private void ChangeTreeMaterials(LevelZone levelZone)
    {
        foreach (Transform treeTransform in treeContainer)
        {
            if (levelZone.treeTrunkMaterial != null)
            {
                treeTransform.GetComponent<Image>().material = levelZone.treeTrunkMaterial;
            }
            if (levelZone.treePalmMaterial != null)
            {
                treeTransform.GetChild(0).GetComponent<Image>().material = levelZone.treePalmMaterial;
            }
        }
    }
}
