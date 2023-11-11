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
    public Dictionary<ItemName, Sprite> itemSprite = new Dictionary<ItemName, Sprite>();
    public ItemName[] itemSprite_ItemName;
    public Sprite[] itemSprite_Sprite;
    private Image[] itemUIImage;
    [SerializeField]private List<TextMeshProUGUI> itemUIText;
    //stateUI를 수정하기 위한 데이터들
    public Slider HPBar; //hp슬라이더 추가 
    public Slider EnergyBar;
    public GameObject[] HungerBar = new GameObject[2];



    private void Awake()
    {
        for(int i=0; i<itemSprite_ItemName.Length; i++)
        {
            itemSprite.Add(itemSprite_ItemName[i], itemSprite_Sprite[i]);
        }
        itemUIImage = new Image[itemUI.Length];
        for(int i=0; i< itemUI.Length; i++)
        {
            //Debug.Log(itemUI[i]);
            itemUIImage[i] = itemUI[i].GetComponent<Image>();
            itemUIText.Add(itemUI[i].GetComponentInChildren<TextMeshProUGUI>());
        }
        player = GameObject.Find("Player");
        playerState = player.GetComponent<State>();
        HungerBar[0].SetActive(false);
        HungerBar[1].SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckItem();
        CheckState();
    }

    void CheckItem()
    {
        for(int i=0; i< itemUI.Length; i++)
        {
            if (playerState.item[i].count == 0)
            {
                itemUIImage[i].color = new Color(itemUIImage[i].color.r, itemUIImage[i].color.g, itemUIImage[i].color.b, 0);
                itemUIImage[i].sprite = null;
                itemUIText[i].text = null;
                continue;
            }
            itemUIImage[i].color = new Color(itemUIImage[i].color.r, itemUIImage[i].color.g, itemUIImage[i].color.b, 255);
            itemUIImage[i].sprite = itemSprite[playerState.item[i].name];
            itemUIText[i].text = playerState.item[i].count.ToString();
        }
    }

    void CheckState()
    {
        HPBar.value = playerState.currentHP / playerState.maxHP;
        EnergyBar.value = playerState.currentEnergy / playerState.maxEnergy;
        switch (playerState.currentHunger)
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
