using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CutSceneController : MonoBehaviour
{
    public GameObject[] cutScene;
    int idx = 0;    

    // Update is called once per frame
    void Update()
    {
        if (idx < cutScene.Length)
        {
            PressToSkip();
        }
    }

    void PressToSkip()
    {
        if(Input.anyKeyDown) {
            cutScene[idx++].SetActive(false);
        }
    }
}
