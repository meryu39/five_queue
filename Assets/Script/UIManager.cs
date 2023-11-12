using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //�ڵ� ���������� �ʿ��� �����͵�
    private GameObject player;
    private State playerState;
    //itemUI�� �����ϱ� ���� �����͵�
    public GameObject[] itemUI;
    public Dictionary<InteractionObjectName, Sprite> itemSprite = new Dictionary<InteractionObjectName, Sprite>();    //itemSprite_ItemName, itemSprite_Sprite������ Dictionary ���·� ���
    public InteractionObjectName[] itemSprite_ItemName;
    public Sprite[] itemSprite_Sprite;
    private Image[] itemUIImage;
    [SerializeField]private List<TextMeshProUGUI> itemUIText;
    //stateUI�� �����ϱ� ���� �����͵�
    public Slider HPBar;        //ü�¹�
    public Slider EnergyBar;    //��¹�
    public GameObject[] HungerBar = new GameObject[2];  //����



    private void Awake()
    {
        //ItemSprite ��ųʸ� �ڷᱸ�� �ʱ�ȭ
        for(int i=0; i<itemSprite_ItemName.Length; i++)
        {
            itemSprite.Add(itemSprite_ItemName[i], itemSprite_Sprite[i]);
        }
        //������ �κ��丮�� ������ ������ �ؽ�Ʈ�� ǥ���ϱ� ���� �޸� Ȯ��
        itemUIImage = new Image[itemUI.Length];
        for(int i=0; i< itemUI.Length; i++)
        {
            //Debug.Log(itemUI[i]);
            itemUIImage[i] = itemUI[i].GetComponent<Image>();
            itemUIText.Add(itemUI[i].GetComponentInChildren<TextMeshProUGUI>());
        }
        //�÷��̾�� �÷��̾� ���� ����
        player = GameObject.Find("Player");
        playerState = player.GetComponent<State>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckItem();        //���� ������ ���� Ȯ��
        CheckState();       //���� �÷��̾� ���� Ȯ��
    }

    void CheckItem()        //�÷��̾ �����ϰ� �ִ� �������� ���¸� Ȯ���Ͽ� ������ UI�� ǥ��
    {
        //������ 1~3ĭ ���� Ȯ��
        for(int i=0; i< itemUI.Length; i++)
        {
            //�������� ��� �Ҹ��� ��� �ش� ������ ĭ ����
            if (playerState.item[i].count == 0)
            {
                itemUIImage[i].color = new Color(itemUIImage[i].color.r, itemUIImage[i].color.g, itemUIImage[i].color.b, 0);
                itemUIImage[i].sprite = null;
                itemUIText[i].text = null;
                continue;
            }
            //�������� �ִ� ��� �ش� ������ ǥ���ϱ�
            itemUIImage[i].color = new Color(itemUIImage[i].color.r, itemUIImage[i].color.g, itemUIImage[i].color.b, 255);
            itemUIImage[i].sprite = itemSprite[playerState.item[i].name];
            itemUIText[i].text = playerState.item[i].count.ToString();
        }
    }

    void CheckState()   //���� �÷��̾� ���¸� Ȯ���Ͽ� �÷��̾� ���� UI ������Ʈ
    {
        HPBar.value = playerState.currentHP / playerState.maxHP;
        EnergyBar.value = playerState.currentEnergy / playerState.maxEnergy;
        switch (playerState.currentHunger)  //��� ���� ���� ��� UI ���� ����
        {
            case 0:
                HungerBar[0].SetActive(false);
                HungerBar[1].SetActive(false);
                break;
            case 1:
                HungerBar[0].SetActive(true);
                HungerBar[1].SetActive(false);
                break;
            case 2:
                HungerBar[0].SetActive(true);
                HungerBar[1].SetActive(true);
                break;
        }
    }

}
