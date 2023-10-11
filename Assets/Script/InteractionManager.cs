using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public enum Category //��ȣ�ۿ� ��ü�� ī�װ�
    {
        WEAPON = 0,
        ITEM
    }

    //InteractionManager�� ȣ���ϱ� ���� �ν��Ͻ�
    static public InteractionManager instance;


    public bool Interact(GameObject interactionObject)
    {
        //��ȣ�ۿ��ϴ� ������Ʈ�� Interaction ��ũ��Ʈ ����
        Interaction interactionInformation = interactionObject.GetComponent<Interaction>();
        //Interaction ��ũ��Ʈ�� ���� ��� Log ����
        if (interactionInformation == null)
        {
            Debug.Log(interactionObject + "�� Interaction ��ũ��Ʈ�� ����.");
            return false;
        }
        //ī�װ��� ���� �Լ� ����
        switch(interactionInformation.category)
        {
            case Category.ITEM:
                Debug.Log("ī�װ� ������ �Լ� ����");
                break;
            case Category.WEAPON:
                Debug.Log("ī�װ� ���� �Լ� ����");
                break;
        }
        //��ȣ�ۿ��� ��ü ����, ���Ŀ� �� ī�װ��� �������� ����� ����
        interactionObject.SetActive(false);
        return true;
    }


    void Awake()
    {
        instance = this;
    }
}
