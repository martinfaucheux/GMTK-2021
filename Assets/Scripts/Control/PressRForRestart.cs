using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressRForRestart : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            LevelLoader.instance.ReloadLevel();
        }
    }
}
