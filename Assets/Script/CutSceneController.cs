using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CutSceneController : MonoBehaviour
{
    public bool CanMove = false;

    public static CutSceneController instance;
    public GameObject[] cutScene;
    int idx = 0;

    private void Awake()
    {
        CutSceneController.instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (idx < cutScene.Length)
        {
            PressToSkip();
        }
        else if(!CanMove)
        {
            CanMove = true;
            SoundManager.instance.PlayBgm(SoundManager.Bgm.VIP, true);
        }
    }

    void PressToSkip()
    {
        if(Input.anyKeyDown) {
            cutScene[idx++].SetActive(false);
            SoundManager.instance.PlaySfx(SoundManager.Sfx.MiniButton);
        }
    }
}
