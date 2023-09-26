using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Silder class 사용하기 위해 추가합니다.


//public float HPbar.value;

public class State : MonoBehaviour
{
    
    public Slider HPbar;
  //  public static State instance; // 다른 스크립트에서 접근하기 위한 인스턴스 변수



    void Awake()
    {
       // instance = this; // State 스크립트의 인스턴스를 저장        
    }

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