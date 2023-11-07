using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI; 



public class State : MonoBehaviour
{
    public float maxHP = 100f; //최대체력 100
    public float maxEnergy = 100f;
    public int maxHunger = 2;
    public float currentHP; //현재 hp
    public float currentEnergy;
    public int currentHunger;
    //재생과 관련된 변수들
    public float HPRegenerationAmount = 2f;
    public float EnergyRegenerationAmount = 5f;
    public float HPRegenerationCondition_EnergyPercent = 0.98f;
    private float lastDecreaseHungerTime;
    public float decreaseHungerCoolTime = 5.0f;

    public Slider HPbar; //hp슬라이더 추가 

    public float PlayerAttackDamage = 25;

    public Item[] item = new Item[3];

    void Awake()
    {
        currentHP = maxHP;
        currentEnergy = maxEnergy;
        currentHunger = maxHunger;
        HPbar.value = currentHP;
        lastDecreaseHungerTime = Time.time;
        UpdateHP();
    }

    private void Update()
    {
        if (currentEnergy >= maxEnergy * HPRegenerationCondition_EnergyPercent)
        {
            SetHP(currentHP + (Time.deltaTime * HPRegenerationAmount));
        }

        if (currentHunger >= maxHunger)
        {
            SetEnergy(currentEnergy + (Time.deltaTime * EnergyRegenerationAmount));
        }
    }

    public void Pdamage(float damage)
    {
        currentHP -= damage;
        UpdateHP();
    }


    private void UpdateHP()
    {
        if (HPbar != null)
        {
            HPbar.value = (float)currentHP / maxHP;
        }
        if(currentHP <0)
        {
            Debug.Log("...꿈이었구나. 조심해야겠다."); //다시하기 이미지출력
        }
        if(Time.time - lastDecreaseHungerTime >= decreaseHungerCoolTime)
        {
            SetHunger(currentHunger - 1);
            lastDecreaseHungerTime = Time.time;
        }
    }

    public bool GetItem(Item obtainedItem)
    {
        for(int i=0; i<3; i++)
        {
            if (item[i].count == 0)
            {
                Debug.Log("null 확인");
                item[i] = obtainedItem;
                return true;
            }
        }
        return false;
    }

    public void SetHP(float value)
    {
        currentHP = value;
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        if(currentHP <= 0)
        {
            currentHP = 0;
            Debug.Log("플레이어 사망");
        }
        UpdateHP();
    }

    public void SetEnergy(float value)
    {
        currentEnergy = value;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        if (currentEnergy <= 0)
        {
            currentEnergy = 0;
        }
    }

    public void SetHunger(int value)
    {
        currentHunger = value;
        if (currentHunger > maxHunger)
        {
            currentHunger = maxHunger;
        }
        if (currentHunger <= 0)
        {
            currentHunger = 0;
        }
    }
}