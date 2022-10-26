using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// <summary>
// This forces to recalculate the layout group when object is loaded
// This can avoid glitches where layout childs are stacked on each other
// </summary>
public class ForceRecalculateLayout : MonoBehaviour
{
    [SerializeField] bool _forcePosition;
    [SerializeField] bool _atEndOfFrame;
    [SerializeField] Vector2 _position;

    void Start()
    {
        if (_atEndOfFrame)
            StartCoroutine(RecalculateCoroutine());
        else
            Recalculate();
    }

    private void Recalculate()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
        if (_forcePosition)
            ((RectTransform)transform).anchoredPosition = _position;
    }

    private IEnumerator RecalculateCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Recalculate();
    }
}
