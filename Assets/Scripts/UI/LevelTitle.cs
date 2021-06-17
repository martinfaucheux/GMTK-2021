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
        _showPos = _rectTransform.position;
        float initSize = _rectTransform.sizeDelta.x;

        _hidePos = new Vector3(-initSize, _showPos.y, _showPos.z);

        _rectTransform.position = _hidePos;
        SetText();
        StartCoroutine(Slide());
    }

    private void SetText(){
        int levelId = LevelLoader.instance.currentLevelId;
        textComponent.SetText("Level " + levelId.ToString());
    }

    private IEnumerator Slide(){
        yield return new WaitForSeconds(waitBeforeShow);
        LeanTween.move(gameObject, _showPos,  slideSpeed);
        yield return new WaitForSeconds(waitBeforeShow + showDuration);
        LeanTween.move(gameObject, _hidePos,  slideSpeed);
    }
}
