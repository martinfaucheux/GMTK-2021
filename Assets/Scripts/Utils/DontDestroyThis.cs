using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyThis : MonoBehaviour
{
    void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }
}
