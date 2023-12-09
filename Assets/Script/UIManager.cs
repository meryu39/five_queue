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
    public TextMeshProUGUI[] itemUIText;
    //stateUI�� �����ϱ� ���� �����͵�
    public Slider HPBar;        //ü�¹�
    public Slider EnergyBar;    //��¹�
    public GameObject[] HungerBar = new GameObject[2];  //����
    //weaponUI�� �����ϱ� ���� �����͵�
    public GameObject weaponUI;
    private Image weaponUIImage;
    public Slider weaponUIDurabilityBar;
    public Dictionary<InteractionObjectName, Sprite> weaponSprite = new Dictionary<InteractionObjectName, Sprite>();
    public Sprite[] weaponSprite_sprite;
    public InteractionObjectName[] weaponSprite_weaponName;
    //��ų ���� ĵ���� 
    public GameObject SkillCanvas;
    private bool CanvasActive = false; 

    private void Awake()
    {
        //������UI ���� �ʱ�ȭ
        for (int i = 0; i < itemSprite_ItemName.Length; i++)
        {
            //ItemSprite ��ųʸ� �ڷᱸ�� �ʱ�ȭ
            itemSprite.Add(itemSprite_ItemName[i], itemSprite_Sprite[i]);
        }
        itemUIImage = new Image[itemUI.Length];
        itemUIText = new TextMeshProUGUI[itemUI.Length];
        for (int i = 0; i < itemUI.Length; i++)
        {
            //Debug.Log(itemUI[i]);
            itemUIImage[i] = itemUI[i].GetComponent<Image>();
            itemUIText[i] = itemUI[i].GetComponentInChildren<TextMeshProUGUI>();
        }
        //weaponUI ���� �ʱ�ȭ
        weaponUIImage = weaponUI.GetComponent<Image>();
        for(int i=0; i<weaponSprite_weaponName.Length; i++)
        {
            weaponSprite.Add(weaponSprite_weaponName[i], weaponSprite_sprite[i]);
        }
        //�÷��̾�� �÷��̾� ���� ����
        player = GameObject.Find("Player");
        playerState = player.GetComponent<State>();
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckItem();        //���� ������ ���� Ȯ��
        CheckWeapon();
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

    private void Update()
    {
        // ĵ������ Ȱ��ȭ�Ǿ� ���� �ʰ� X Ű�� ������ ��
        if (!CanvasActive && Input.GetKeyDown(KeyCode.X))
        {
            SkillCanvas.SetActive(true);
            CanvasActive = true;
            Debug.Log("��ųUI�� Ȱ��ȭ�մϴ�");
        }

        // ĵ������ Ȱ��ȭ�Ǿ� �ְ� Escape Ű �Ǵ� X Ű�� ������ ��
        else if (CanvasActive && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X)))
        {
            SkillCanvas.SetActive(false);
            CanvasActive = false;
            Debug.Log("��ųUI�� ��Ȱ��ȭ�մϴ�");
        }
    }

    void CheckWeapon()
    {
        if (playerState.auxiliaryWeapon.count <= 0)
        {
            weaponUIImage.color = new Color(weaponUIImage.color.r, weaponUIImage.color.g, weaponUIImage.color.b, 0);
            weaponUIImage.sprite = null;
            weaponUIDurabilityBar.value = 0;
            weaponUIDurabilityBar.gameObject.SetActive(false);
            return;
        }
        weaponUIImage.color = new Color(weaponUIImage.color.r, weaponUIImage.color.g, weaponUIImage.color.b, 255);
        weaponUIImage.sprite = weaponSprite[playerState.auxiliaryWeapon.name];
        weaponUIDurabilityBar.gameObject.SetActive(true);
        weaponUIDurabilityBar.value = (float)playerState.auxiliaryWeapon.count / (float)InteractionManager.instance.weaponAcquiredDurability[playerState.auxiliaryWeapon.name];
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
