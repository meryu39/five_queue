using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void OnClickNewGame()
    {
        SceneManager.LoadScene("map");
    }
    public void OnClickExit()
    {
        Application.Quit();
        Debug.Log("Click");
    }
}
