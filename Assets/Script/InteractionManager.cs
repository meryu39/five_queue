using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using UnityEngine;

public enum ItemCategory //상호작용 물체의 카테고리
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

    //InteractionManager를 호출하기 위한 인스턴스
    static public InteractionManager instance;
    private GameObject player;


    public bool Interact(GameObject interactionObject)
    {
        //상호작용하는 오브젝트의 Interaction 스크립트 정보
        Interaction interactionInformation = interactionObject.GetComponent<Interaction>();
        Item obtainedItem = null;
        //Interaction 스크립트가 없을 경우 Log 수행
        if (interactionInformation == null)
        {
            Debug.Log(interactionObject + "에 Interaction 스크립트가 없다.");
            return false;
        }
        //카테고리에 따른 함수 수행
        switch(interactionInformation.itemCategory)
        {
            //아이템 종류에 따라 개수를 다르게 획득
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
                Debug.Log("카테고리 무기 함수 수행");
                break;
        }
        //상호작용한 물체 삭제, 추후에 각 카테고리별 아이템이 생기면 삭제```
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