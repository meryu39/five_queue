using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Silder class ����ϱ� ���� �߰��մϴ�.



public float HPbar.value;

public class State : MonoBehaviour
{
    Slider HPbar;
    
   

    
    void Start()
    {
        HPbar = GetComponent<Slider>();

    }


    void Update()
    {
        

        if (HPbar.value <= 0)
            transform.Find("Fill Area").gameObject.SetActive(false);
        else
            transform.Find("Fill Area").gameObject.SetActive(true);
    }
}