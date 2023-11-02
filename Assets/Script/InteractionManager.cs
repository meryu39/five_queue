using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using UnityEngine;

public enum ItemCategory //��ȣ�ۿ� ��ü�� ī�װ�
{
    WEAPON = 0,
    ITEM
}
public enum ItemName
{
    BANDAGE, PAINKILLER, EPINEPHRINE, CAN, CUPRAMEN
}

public class InteractionManager : MonoBehaviour
{

    //InteractionManager�� ȣ���ϱ� ���� �ν��Ͻ�
    static public InteractionManager instance;
    private GameObject player;


    public bool Interact(GameObject interactionObject)
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
            //������ ������ ���� ������ �ٸ��� ȹ��
            case ItemCategory.ITEM:
                if(interactionInformation.itemName == ItemName.BANDAGE)
                {
                    obtainedItem = new Item(ItemCategory.ITEM, ItemName.BANDAGE, ItemAcquiredAmount.bandage);
                }
                else if (interactionInformation.itemName == ItemName.PAINKILLER)
                {
                    obtainedItem = new Item(ItemCategory.ITEM, ItemName.PAINKILLER, ItemAcquiredAmount.painkiller);
                }
                else if (interactionInformation.itemName == ItemName.EPINEPHRINE)
                {
                    obtainedItem = new Item(ItemCategory.ITEM, ItemName.EPINEPHRINE, ItemAcquiredAmount.epinephrine);
                }
                else if (interactionInformation.itemName == ItemName.CAN)
                {
                    obtainedItem = new Item(ItemCategory.ITEM, ItemName.CAN, ItemAcquiredAmount.can);
                }
                else if (interactionInformation.itemName == ItemName.BANDAGE)
                {
                    obtainedItem = new Item(ItemCategory.ITEM, ItemName.BANDAGE, ItemAcquiredAmount.cupramen);
                }
                player.GetComponentInChildren<State>().GetItem(obtainedItem);
                break;
            case ItemCategory.WEAPON:
                Debug.Log("ī�װ� ���� �Լ� ����");
                break;
        }
        //��ȣ�ۿ��� ��ü ����, ���Ŀ� �� ī�װ��� �������� ����� ����```
        interactionObject.SetActive(false);
        return true;
    }
    

    void Awake()
    {
        instance = this;
        player = GameObject.Find("Player");
    }
}

[System.Serializable]
public class Item
{
    public ItemCategory category;
    public ItemName name;
    public int count;
    public Item(ItemCategory category, ItemName name, int count = 1)
    {
        this.category = category;
        this.name = name;
        this.count = count;
    }
};

public static class ItemAcquiredAmount
{
    public static int bandage = 4;
    public static int painkiller = 3;
    public static int epinephrine = 1;
    public static int can = 1;
    public static int cupramen = 1;
};