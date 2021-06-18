using System.Collections;
using UnityEngine;
using TMPro;

public class LevelTitle : MonoBehaviour
{

    [SerializeField] float waitBeforeShow;
    [SerializeField] float showDuration;
    [SerializeField] float slideSpeed;
    [SerializeField] TextMeshProUGUI textComponent;
    private Vector3 _showPos;
    private Vector3 _hidePos;
    private RectTransform _rectTransform;

    void Start()
    {
        _rectTransform = (RectTransform) transform;
        _showPos = _rectTransform.anchoredPosition;
        float initSize = _rectTransform.sizeDelta.x;

        _hidePos = _showPos - new Vector3(1.5f * initSize, 0f, 0f);

        _rectTransform.anchoredPosition = _hidePos;
        SetText();
        StartCoroutine(Slide());
    }

    private void SetText(){
        int levelId = LevelLoader.instance.currentLevelId;
        textComponent.SetText("Level " + levelId.ToString());
    }

    private IEnumerator Slide(){
        yield return new WaitForSeconds(waitBeforeShow);
        LeanTween.move(_rectTransform, _showPos,  slideSpeed);
        yield return new WaitForSeconds(waitBeforeShow + showDuration);
        LeanTween.move(_rectTransform, _hidePos,  slideSpeed);
    }
}
