using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 



public class State : MonoBehaviour
{
    public int MaxHP = 100; //�ִ�ü�� 100
    public int currentHP; //���� hp

    public Slider HPbar; //hp�����̴� �߰� 


    void Awake()
    {
        HPbar = GetComponent<Slider>(); //�����̵� �����ʹ� �Ҵ�
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