using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI; 



public class State : MonoBehaviour
{
    //ĳ���� ��ų ��Ƽ�� 
    public int active_e = -1;
    public int active_shift = -1;

    public float exp; //ĳ���� ����ġ
    public int skill_exp = 10; //��ų����Ʈ
    public float maxHP = 100f;          //�ִ� ü��
    public float maxEnergy = 100f;      //�ִ� ���
    public int maxHunger = 2;           //�ִ� ���
    public float currentHP;             //���� ü��         
    public float currentEnergy;         //���� ���
    public int currentHunger;           //���� ���
    //����� ���õ� ������
    public float HPRegenerationAmount = 2f;                     //��� �� �ʴ� ü�� ȸ����
    public float EnergyRegenerationAmount = 3f;                 //��� �� �ʴ� ��� ȸ����
    public float HPRegenerationCondition_EnergyPercent = 1.0f;  //ü�� ��� �ߵ� ������ �Ǵ� ����� �ۼ�Ʈ, 0~1���� ���̴�.
    private float lastDecreaseHungerTime;                       //���������� ��Ⱑ ���ҵ� �ð�
    public float decreaseHungerCoolTime = 120.0f;               //��Ⱑ ���ҵǴ� �ֱ�
    //������, ����� ���õ� ������
    public Item[] item = new Item[3];                           //���� ���� ���� ������
    public Item auxiliaryWeapon = new Item();

    public float PlayerAttackDamage = 25;
    public int testIndex = 0;
    

    void Awake()
    {
        //�÷��̾� ���� �ʱ�ȭ
        currentHP = maxHP;
        currentEnergy = maxEnergy;
        currentHunger = maxHunger;
        lastDecreaseHungerTime = Time.time;
    }

    private void Update()
    {
        //���� ����� �ִ��� * HPRegenerationCondition_EnergyPercent�� ��� �ʴ� HPRegenerationAmount��ŭ ü�� ȸ��
        if (currentEnergy >= maxEnergy * HPRegenerationCondition_EnergyPercent)
        {
            SetHP(currentHP + (Time.deltaTime * HPRegenerationAmount));
        }
        //���� ��Ⱑ �ִ� ����� ��� �ʴ� EnergyRegenerationAmount��ŭ ��� ȸ��
        if (currentHunger >= maxHunger)
        {
            SetEnergy(currentEnergy + (Time.deltaTime * EnergyRegenerationAmount));
        }
        //��� ���� �ֱⰡ ���� ��� ��� 
        if (Time.time - lastDecreaseHungerTime >= decreaseHungerCoolTime)
        {
            SetHunger(currentHunger - 1);
            lastDecreaseHungerTime = Time.time;
        }
        if(exp > 100)
        {
            exp %= 100;
            skill_exp++;
        }
        //Debug.Log(currentHP + ", " + ++testIndex);
    }


    
    public bool GetItem(Item obtainedItem)  //�������� ȹ���ϴ� �Լ��̴�. ���� ����ִ� �κ��丮�� Ȯ���Ͽ� �� �κ��丮�� �������� �ִ´�.
    {
        if (obtainedItem.category == InteractionObjectCategory.ITEM)
        {
            for (int i = 0; i < 3; i++)
            {
                //�������� ���� �κ��丮�� �߰��� ��� ȹ��
                if (item[i].count == 0)
                {
                    item[i] = obtainedItem;
                    return true;
                }
            }
            //�κ��丮�� ������ ��� false ����
            return false;
        }
        else if(obtainedItem.category == InteractionObjectCategory.WEAPON)
        {
            auxiliaryWeapon = obtainedItem;
            return true;
        }
        return false;
    }

    public void SetHP(float value)      //HP�� �����ϴ� �Լ��̴�. ���� ü���� value�� �����ȴ�. ü�� ����/���Ҹ� �ϰ� ������ value�� 'state.currentHP +- ��ȭ��'�� ������ �ȴ�.
    {
        currentHP = value;

        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        if(currentHP <= 0)
        {
            currentHP = 0;
            Debug.Log("�÷��̾� ���");
        }
    }

    public void SetEnergy(float value)  //����� �����ϴ� �Լ��̴�. ���� ����� value�� �����ȴ�. ��� ����/���Ҹ� �ϰ� ������ value�� 'state.currentEnergy +- ��ȭ��'�� ������ �ȴ�.
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

    public void SetHunger(int value)    //��⸦ �����ϴ� �Լ��̤���. ���� ��Ⱑ value�� �����ȴ�. ��� ����/���Ҹ� �ϰ� ������ value�� 'state.currentHunger +- ��ȭ��'�� ������ �ȴ�.
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