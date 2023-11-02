using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI; 



public class State : MonoBehaviour
{
    public int MaxHP = 100; //�ִ�ü�� 100
    public int currentHP; //���� hp

    public Slider HPbar; //hp�����̴� �߰� 

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
            Debug.Log("...���̾�����. �����ؾ߰ڴ�."); //�ٽ��ϱ� �̹������
        }
    }

    public void GetItem(Item obtainedItem)
    {
        for(int i=0; i<3; i++)
        {
            if (item[i].count == 0)
            {
                Debug.Log("null Ȯ��");
                item[i] = obtainedItem;
                break;
            }
        }
    }
}