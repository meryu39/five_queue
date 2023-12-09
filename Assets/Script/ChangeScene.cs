using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public GameObject credit;

    void Update()
    {
        if (Input.anyKeyDown&&credit.activeSelf == true)
        {
            credit.SetActive(false);
        }
    }
    public void OnClickNewGame()
    {
        SceneManager.LoadScene("map");
    }

    public void OnClickCredit()
    {
        credit.SetActive(true);
    }

    public void OnClickExit()
    {
        Application.Quit();
        Debug.Log("Click");
    }
}
