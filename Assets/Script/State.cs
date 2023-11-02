using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI; 



public class State : MonoBehaviour
{
    public int MaxHP = 100; //최대체력 100
    public int currentHP; //현재 hp

    public Slider HPbar; //hp슬라이더 추가 

    public int PlayerAttackDamage = 25;

    public Item[] item;

    void Start()
    {
        item = new Item[3];
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
        if(currentHP <0)
        {
            Debug.Log("...꿈이었구나. 조심해야겠다."); //다시하기 이미지출력
        }
    }

    public void GetItem(Item obtainedItem)
    {
        for(int i=0; i<3; i++)
        {
            if (item[i].count == 0)
            {
                Debug.Log("null 확인");
                item[i] = obtainedItem;
                break;
            }
        }
    }
}