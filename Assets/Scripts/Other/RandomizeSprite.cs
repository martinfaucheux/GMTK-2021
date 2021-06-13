using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSprite : MonoBehaviour
{
    [SerializeField] Sprite[] spirteList;
    void Start()
    {
        if(spirteList.Length > 0){
            Sprite sprite = spirteList[Random.Range (0, spirteList.Length)];
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
