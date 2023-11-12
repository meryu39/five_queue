using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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
    private const int bandageAcquiredAmount = 4;
    private const int painkillerAcquiredAmount = 3;
    private const int epinephrineAcquiredAmount = 1;
    private const int canAcquiredAmount = 1;
    private const int cupramenAcquiredAmount = 1;

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
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.BANDAGE, bandageAcquiredAmount);
                }
                else if (interactionInformation.itemName == InteractionObjectName.PAINKILLER)
                {
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.PAINKILLER, painkillerAcquiredAmount);
                }
                else if (interactionInformation.itemName == InteractionObjectName.EPINEPHRINE)
                {
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.EPINEPHRINE, epinephrineAcquiredAmount);
                }
                else if (interactionInformation.itemName == InteractionObjectName.CAN)
                {
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.CAN, canAcquiredAmount);
                }
                else if (interactionInformation.itemName == InteractionObjectName.CUPRAMEN)
                {
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.CUPRAMEN, cupramenAcquiredAmount);
                }
                if(!player.GetComponentInChildren<State>().GetItem(obtainedItem))   //State.cs�� GetItem�� false�� ��� ������ ȹ�濡 �����Ѵ�.
                {
                    Debug.Log("�κ��丮 ����!");
                    return false;
                }
                break;
            //WEAPON ī�װ��� �������� ȹ���� ���
            case InteractionObjectCategory.WEAPON:
                Debug.Log("ī�װ� ���� �Լ� ����");
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
