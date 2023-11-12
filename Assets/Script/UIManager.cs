using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //코드 전반적으로 필요한 데이터들
    private GameObject player;
    private State playerState;
    //itemUI를 수정하기 위한 데이터들
    public GameObject[] itemUI;
    public Dictionary<InteractionObjectName, Sprite> itemSprite = new Dictionary<InteractionObjectName, Sprite>();    //itemSprite_ItemName, itemSprite_Sprite정보를 Dictionary 형태로 사용
    public InteractionObjectName[] itemSprite_ItemName;
    public Sprite[] itemSprite_Sprite;
    private Image[] itemUIImage;
    [SerializeField]private List<TextMeshProUGUI> itemUIText;
    //stateUI를 수정하기 위한 데이터들
    public Slider HPBar;        //체력바
    public Slider EnergyBar;    //기력바
    public GameObject[] HungerBar = new GameObject[2];  //허기바



    private void Awake()
    {
        //ItemSprite 딕셔너리 자료구조 초기화
        for(int i=0; i<itemSprite_ItemName.Length; i++)
        {
            itemSprite.Add(itemSprite_ItemName[i], itemSprite_Sprite[i]);
        }
        //아이템 인벤토리에 아이템 사진과 텍스트를 표시하기 위한 메모리 확보
        itemUIImage = new Image[itemUI.Length];
        for(int i=0; i< itemUI.Length; i++)
        {
            //Debug.Log(itemUI[i]);
            itemUIImage[i] = itemUI[i].GetComponent<Image>();
            itemUIText.Add(itemUI[i].GetComponentInChildren<TextMeshProUGUI>());
        }
        //플레이어와 플레이어 스탯 저장
        player = GameObject.Find("Player");
        playerState = player.GetComponent<State>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckItem();        //현재 아이템 상태 확인
        CheckState();       //현재 플레이어 스탯 확인
    }

    void CheckItem()        //플레이어가 보유하고 있는 아이템의 상태를 확인하여 아이템 UI에 표시
    {
        //아이템 1~3칸 전부 확인
        for(int i=0; i< itemUI.Length; i++)
        {
            //아이템을 모두 소모한 경우 해당 아이템 칸 비우기
            if (playerState.item[i].count == 0)
            {
                itemUIImage[i].color = new Color(itemUIImage[i].color.r, itemUIImage[i].color.g, itemUIImage[i].color.b, 0);
                itemUIImage[i].sprite = null;
                itemUIText[i].text = null;
                continue;
            }
            //아이템이 있는 경우 해당 아이템 표시하기
            itemUIImage[i].color = new Color(itemUIImage[i].color.r, itemUIImage[i].color.g, itemUIImage[i].color.b, 255);
            itemUIImage[i].sprite = itemSprite[playerState.item[i].name];
            itemUIText[i].text = playerState.item[i].count.ToString();
        }
    }

    void CheckState()   //현재 플레이어 상태를 확인하여 플레이어 스탯 UI 업데이트
    {
        HPBar.value = playerState.currentHP / playerState.maxHP;
        EnergyBar.value = playerState.currentEnergy / playerState.maxEnergy;
        switch (playerState.currentHunger)  //허기 값에 따라 허기 UI 상태 수정
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
