using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using UnityEngine;

public enum ItemCategory //��ȣ�ۿ��ϴ� ������Ʈ�� ī�װ� ����
{
    NULL = -1,
    WEAPON = 0,
    ITEM
}
public enum ItemName    //��ȣ�ۿ��ϴ� ������Ʈ�� �̸�
{
    NULL = -1, 
    /*Item*/ BANDAGE, PAINKILLER, EPINEPHRINE, CAN, CUPRAMEN,
    /*Weapon*/ SCALPEL, PIPE, BLOODPACK, FIREEXTINGUISHER
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
        switch(interactionInformation.itemCategory)
        {
            //������ ������ �ƴ� ��� ����
            case ItemCategory.NULL:
                break;
            //ITEM ī�װ��� �������� ȹ���� ��� ��ȣ�ۿ��� �������� ������ GetItem���� ������.
            case ItemCategory.ITEM:
                if(interactionInformation.itemName == ItemName.BANDAGE)
                {
                    obtainedItem = new Item(ItemCategory.ITEM, ItemName.BANDAGE, bandageAcquiredAmount);
                }
                else if (interactionInformation.itemName == ItemName.PAINKILLER)
                {
                    obtainedItem = new Item(ItemCategory.ITEM, ItemName.PAINKILLER, painkillerAcquiredAmount);
                }
                else if (interactionInformation.itemName == ItemName.EPINEPHRINE)
                {
                    obtainedItem = new Item(ItemCategory.ITEM, ItemName.EPINEPHRINE, epinephrineAcquiredAmount);
                }
                else if (interactionInformation.itemName == ItemName.CAN)
                {
                    obtainedItem = new Item(ItemCategory.ITEM, ItemName.CAN, canAcquiredAmount);
                }
                else if (interactionInformation.itemName == ItemName.CUPRAMEN)
                {
                    obtainedItem = new Item(ItemCategory.ITEM, ItemName.CUPRAMEN, cupramenAcquiredAmount);
                }
                if(!player.GetComponentInChildren<State>().GetItem(obtainedItem))   //State.cs�� GetItem�� false�� ��� ������ ȹ�濡 �����Ѵ�.
                {
                    Debug.Log("�κ��丮 ����!");
                    return false;
                }
                break;
            //WEAPON ī�װ��� �������� ȹ���� ���
            case ItemCategory.WEAPON:
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
    public ItemCategory category;
    public ItemName name;
    public int count;

    public Item()
    {
        category = ItemCategory.NULL;
        name = ItemName.NULL;
        count = 0;
    }
    public Item(ItemCategory category, ItemName name, int count)
    {
        this.category = category;
        this.name = name;
        this.count = count;
    }
};
