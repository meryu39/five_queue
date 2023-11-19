using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum InteractionObjectCategory //��ȣ�ۿ��ϴ� ������Ʈ�� ī�װ� ����
{
    NULL = -1,
    WEAPON = 0,
    ITEM,
    NPC
}
public enum InteractionObjectName    //��ȣ�ۿ��ϴ� ������Ʈ�� �̸�
{
    NULL = -1, 
    /*Item*/     BANDAGE, PAINKILLER, EPINEPHRINE, CAN, CUPRAMEN,
    /*Weapon*/   SCALPEL, PIPE, BLOODPACK, FIREEXTINGUISHER,
    /*NPC*/      ELEVATOR
}

public class InteractionManager : MonoBehaviour
{

    //��𼭳� InteractionManager�� ȣ���ϱ� ���� instance�̴�. ��� �����̵� InteractionManager.instance�� ����ϸ� InteractionManager�� ������ �� �ְ� �ȴ�.
    static public InteractionManager instance;
    private GameObject player;
    //�� �����ۺ� ȹ�� ����
    public Dictionary<InteractionObjectName, int> itemAcquiredAmount= new Dictionary<InteractionObjectName, int>()
    {
        { InteractionObjectName.BANDAGE, 4 },
        { InteractionObjectName.PAINKILLER, 3 },
        { InteractionObjectName.EPINEPHRINE, 1 },
        { InteractionObjectName.CAN, 1 },
        { InteractionObjectName.CUPRAMEN, 1 }
    };
    //���⺰ ȹ�� ��������
    public Dictionary<InteractionObjectName, int> weaponAcquiredDurability = new Dictionary<InteractionObjectName, int>()
    {
        { InteractionObjectName.SCALPEL, 1},
        { InteractionObjectName.PIPE, 5 },
        { InteractionObjectName.BLOODPACK, 10 },
        { InteractionObjectName.FIREEXTINGUISHER, 10 }
    };


    void Awake()
    {
        instance = this;
        player = GameObject.Find("Player");
    }

    public bool Interact(GameObject interactionObject)  //FŰ�� ���� ��ȣ�ۿ�� ȣ��Ǵ� �Լ��̴�.
    {
        
        //��ȣ�ۿ��ϴ� ������Ʈ�� Interaction ��ũ��Ʈ ����
        Interaction interactionInformation = interactionObject.GetComponent<Interaction>();
        Item obtainedItem = null;
        Debug.Log("check1");
        //Interaction ��ũ��Ʈ�� ���� ��� Log ����
        if (interactionInformation == null)
        {
            Debug.Log(interactionObject + "�� Interaction ��ũ��Ʈ�� ����.");
            return false;
        }
        //ī�װ��� ���� �Լ� ����
        switch(interactionInformation.Category)
        {
            //������ ������ �ƴ� ��� ����
            case InteractionObjectCategory.NULL:
                break;
            //ITEM ī�װ��� �������� ȹ���� ��� ��ȣ�ۿ��� �������� ������ GetItem���� ������.
            case InteractionObjectCategory.ITEM:
                if(interactionInformation.itemName == InteractionObjectName.BANDAGE)
                {
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.BANDAGE, itemAcquiredAmount[InteractionObjectName.BANDAGE]);
                }
                else if (interactionInformation.itemName == InteractionObjectName.PAINKILLER)
                {
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.PAINKILLER, itemAcquiredAmount[InteractionObjectName.PAINKILLER]);
                }
                else if (interactionInformation.itemName == InteractionObjectName.EPINEPHRINE)
                {
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.EPINEPHRINE, itemAcquiredAmount[InteractionObjectName.EPINEPHRINE]);
                }
                else if (interactionInformation.itemName == InteractionObjectName.CAN)
                {
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.CAN, itemAcquiredAmount[InteractionObjectName.CAN]);
                }
                else if (interactionInformation.itemName == InteractionObjectName.CUPRAMEN)
                {
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.CUPRAMEN, itemAcquiredAmount[InteractionObjectName.CUPRAMEN]);
                }
                if(!player.GetComponentInChildren<State>().GetItem(obtainedItem))   //State.cs�� GetItem�� false�� ��� ������ ȹ�濡 �����Ѵ�.
                {
                    Debug.Log("�κ��丮 ����!");
                    return false;
                }
                break;
            //WEAPON ī�װ��� �������� ȹ���� ���
            case InteractionObjectCategory.WEAPON:
                if(interactionInformation.itemName == InteractionObjectName.SCALPEL)
                {
                    obtainedItem = new Item(InteractionObjectCategory.WEAPON, InteractionObjectName.SCALPEL, weaponAcquiredDurability[InteractionObjectName.SCALPEL]);
                }
                else if (interactionInformation.itemName == InteractionObjectName.PIPE)
                {
                    obtainedItem = new Item(InteractionObjectCategory.WEAPON, InteractionObjectName.PIPE, weaponAcquiredDurability[InteractionObjectName.PIPE]);
                }
                else if(interactionInformation.itemName == InteractionObjectName.BLOODPACK)
                {
                    obtainedItem = new Item(InteractionObjectCategory.WEAPON, InteractionObjectName.BLOODPACK, weaponAcquiredDurability[InteractionObjectName.BLOODPACK]);
                }
                else if (interactionInformation.itemName == InteractionObjectName.FIREEXTINGUISHER)
                {
                    obtainedItem = new Item(InteractionObjectCategory.WEAPON, InteractionObjectName.FIREEXTINGUISHER, weaponAcquiredDurability[InteractionObjectName.FIREEXTINGUISHER]);
                }
                player.GetComponentInChildren<State>().GetItem(obtainedItem);
                break;
        }
        //��ȣ�ۿ��� ��ü ����, ���Ŀ� �� ī�װ��� �������� ����� ����```
        interactionObject.SetActive(false);
        return true;
    }
    

    
}

[System.Serializable]
public class Item   //������ Ŭ����
{

    public InteractionObjectCategory category;
    public InteractionObjectName name;
    public int count;

    public Item()
    {
        category = InteractionObjectCategory.NULL;
        name = InteractionObjectName.NULL;
        count = 0;
    }
    public Item(InteractionObjectCategory category, InteractionObjectName name, int count)
    {
        this.category = category;
        this.name = name;
        this.count = count;
    }
};
