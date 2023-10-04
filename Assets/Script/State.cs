using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 



public class State : MonoBehaviour
{
    public int MaxHP = 100; //최대체력 100
    public int currentHP; //현재 hp

    public Slider HPbar; //hp슬라이더 추가 


    void Awake()
    {
        HPbar = GetComponent<Slider>(); //슬라이드 컴포터는 할당
    }


    void Start()
    {

        currentHP = MaxHP;
        HPbar.value = currentHP;
        UpdateHP();
    }



    public void Pdamage(int damage)
    {
        currentHP -= damage;
        UpdateHP();
    }


    private void UpdateHP()
    {
        if (HPbar != null)
        {
            HPbar.value = (float)currentHP / MaxHP;
        }
    }
}