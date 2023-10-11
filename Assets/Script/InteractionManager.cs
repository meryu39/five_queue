using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public enum Category //상호작용 물체의 카테고리
    {
        WEAPON = 0,
        ITEM
    }

    //InteractionManager를 호출하기 위한 인스턴스
    static public InteractionManager instance;


    public bool Interact(GameObject interactionObject)
    {
        //상호작용하는 오브젝트의 Interaction 스크립트 정보
        Interaction interactionInformation = interactionObject.GetComponent<Interaction>();
        //Interaction 스크립트가 없을 경우 Log 수행
        if (interactionInformation == null)
        {
            Debug.Log(interactionObject + "에 Interaction 스크립트가 없다.");
            return false;
        }
        //카테고리에 따른 함수 수행
        switch(interactionInformation.category)
        {
            case Category.ITEM:
                Debug.Log("카테고리 아이템 함수 수행");
                break;
            case Category.WEAPON:
                Debug.Log("카테고리 무기 함수 수행");
                break;
        }
        //상호작용한 물체 삭제, 추후에 각 카테고리별 아이템이 생기면 삭제
        interactionObject.SetActive(false);
        return true;
    }


    void Awake()
    {
        instance = this;
    }
}
