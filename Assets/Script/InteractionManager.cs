using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using UnityEngine;

public enum ItemCategory //상호작용하는 오브젝트의 카테고리 정보
{
    NULL = -1,
    WEAPON = 0,
    ITEM
}
public enum ItemName    //상호작용하는 오브젝트의 이름
{
    NULL = -1, 
    /*Item*/ BANDAGE, PAINKILLER, EPINEPHRINE, CAN, CUPRAMEN,
    /*Weapon*/ SCALPEL, PIPE, BLOODPACK, FIREEXTINGUISHER
}

public class InteractionManager : MonoBehaviour
{

    //어디서나 InteractionManager를 호출하기 위한 instance이다. 어디 파일이든 InteractionManager.instance를 사용하면 InteractionManager에 접근할 수 있게 된다.
    static public InteractionManager instance;
    private GameObject player;
    //각 아이템별 획득 개수
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

    public bool Interact(GameObject interactionObject)  //F키를 눌러 상호작용시 호출되는 함수이다.
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
            //아이템 종류가 아닌 경우 리턴
            case ItemCategory.NULL:
                break;
            //ITEM 카테고리의 아이템을 획득한 경우 상호작용한 아이템의 정보를 GetItem으로 보낸다.
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
                if(!player.GetComponentInChildren<State>().GetItem(obtainedItem))   //State.cs의 GetItem이 false인 경우 아이템 획득에 실패한다.
                {
                    Debug.Log("인벤토리 꽉참!");
                    return false;
                }
                break;
            //WEAPON 카테고리의 아이템을 획득한 경우
            case ItemCategory.WEAPON:
                Debug.Log("카테고리 무기 함수 수행");
                break;
        }
        //상호작용한 물체 삭제, 추후에 각 카테고리별 아이템이 생기면 삭제```
        interactionObject.SetActive(false);
        return true;
    }
    

    
}

[System.Serializable]
public class Item   //아이템 클래스
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
