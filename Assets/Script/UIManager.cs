using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject player;
    private State playerState;
    //itemUI를 수정하기 위한 데이터들
    public GameObject[] itemUI;
    public Dictionary<ItemName, Sprite> itemSprite = new Dictionary<ItemName, Sprite>();
    public ItemName[] itemSprite_ItemName;
    public Sprite[] itemSprite_Sprite;
    [SerializeField]
    private Image[] itemUIImage;
    [SerializeField]
    private List<TextMeshProUGUI> itemUIText;



    private void Awake()
    {
        for(int i=0; i<itemSprite_ItemName.Length; i++)
        {
            itemSprite.Add(itemSprite_ItemName[i], itemSprite_Sprite[i]);
        }
        itemUIImage = new Image[itemUI.Length];
        for(int i=0; i< itemUI.Length; i++)
        {
            Debug.Log(itemUI[i]);
            itemUIImage[i] = itemUI[i].GetComponent<Image>();
            itemUIText.Add(itemUI[i].GetComponentInChildren<TextMeshProUGUI>());
        }
        player = GameObject.Find("Player");
        playerState = player.GetComponent<State>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckItem();
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

}
