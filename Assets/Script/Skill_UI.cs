using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill_UI : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Button button7;

    public TMPro.TMP_Text skilltext;
    public TMPro.TMP_Text level;


    public string[] skill_text = { "[�Ű���ȭ]", "[�����ҵ�]", "[���� ���� ����]", "[��ȣ��]",
                                   "[���ι���]", "[���� ó��]", "[���� ����]" };


    public bool[] buttonClicked = new bool[7];

    public int active_e = -1;
    public int active_shift = -1;

    public Sprite e_skill;
    public Sprite shift_skill;
    public Sprite get_skill;

    private State state;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player"); // ������ �±׸� ����Ͽ� ã��
        state = player.GetComponent<State>();


    }
    private void Start()
    {
        Debug.Log("Skill_UI ��ũ��Ʈ ���۵�");

        // �� ��ư�� �̺�Ʈ Ʈ���� �߰�
        AddEventTrigger(button1, 0);
        AddEventTrigger(button2, 1);
        AddEventTrigger(button3, 2);
        AddEventTrigger(button4, 3);
        AddEventTrigger(button5, 4);
        AddEventTrigger(button6, 5);
        AddEventTrigger(button7, 6);
    }

    private void Update()
    {
        level.text = state.skill_exp.ToString();
    }
    private void AddEventTrigger(Button button, int index)
    {
        // ��ư�� Ŭ�� �̺�Ʈ �߰�
        button.onClick.AddListener(() => OnButtonClick(index));

        // ���콺�� ��ư ���� �ö� �� �̺�Ʈ �߰�
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        pointerEnter.callback.AddListener((data) => { OnButtonHover(index); });
        trigger.triggers.Add(pointerEnter);
    }

    private void OnButtonHover(int index)
    {
        Print_text(index);
    }


    void exp_learn(int learn_cost)
    {
        if(state.skill_exp >= learn_cost)
        {
            state.skill_exp -= learn_cost;
            
        }
        else
        {
            Debug.Log("exp�� �����ؼ� �����");
        }
    }
    public void OnButtonClick(int index)
    {

        if (!buttonClicked[index])
        {
            // Ŭ���� ��ư�� �̹��� ���� �� �÷��� ����
            switch (index)
            {
                case 0:
                    exp_learn(3);
                    button1.image.sprite = get_skill;
                    buttonClicked[0] = true;
                    break;
                case 1:
                    exp_learn(3);
                    button2.image.sprite = get_skill;
                    buttonClicked[1] = true;
                    break;
                case 2:
                    button3.image.sprite = get_skill;
                    buttonClicked[2] = true;
                    break;
                case 3:
                    exp_learn(3);
                    button4.image.sprite = get_skill;
                    buttonClicked[3] = true;
                    break;
                case 4:
                    exp_learn(3);
                    button5.image.sprite = get_skill;
                    buttonClicked[4] = true;
                    break;
                case 5:
                    exp_learn(2);
                    button6.image.sprite = get_skill;
                    buttonClicked[5] = true;
                    break;
                case 6:
                    exp_learn(4);
                    button7.image.sprite = get_skill;
                    buttonClicked[6] = true;
                    break;
            }
        }

    }

    public void SkillActive(int index)
    {
        if (buttonClicked[index])
        {
            // Ŭ���� ��ư�� �̹��� ���� �� �÷��� ����
            if (Input.GetMouseButtonDown(1)) 
            {
                if (active_e != -1)
                {
                    Debug.Log("e�����");
                    ChangeSprite(active_e, get_skill);

                }
                Debug.Log("e��ų");
                ChangeSprite(index, e_skill);

                active_e = index;
            }
            // �� �ܿ��� ������ ���콺 ��ư Ŭ�� (���� ����)
            else if (Input.GetMouseButtonDown(0))  
            {
                if (active_shift != -1)
                {
                    Debug.Log("����Ʈ�����");
                    ChangeSprite(active_shift, get_skill);

                }
                Debug.Log("����Ʈ��ų");
                ChangeSprite(index, shift_skill);

                active_shift = index;
            }
        }
    }




    private void ChangeSprite(int index, Sprite newSprite)
    {
        // ��������Ʈ ����
        switch (index)
        {
            case 0:
                button1.image.sprite = newSprite;
                break;
            case 1:
                button2.image.sprite = newSprite;
                break;
            case 2:
                button3.image.sprite = newSprite;
                break;
            case 3:
                button4.image.sprite = newSprite;
                break;
            case 4:
                button5.image.sprite = newSprite;
                break;
            case 5:
                button6.image.sprite = newSprite;
                break;
            case 6:
                button7.image.sprite = newSprite;
                break;
        }
    }

    public void Print_text(int index)
    {
        // �ؽ�Ʈ ���
        skilltext.text = skill_text[index];
    }

    public void Print_text_NULL()
    {
        // �ؽ�Ʈ �������� ����
        skilltext.text = " ";
    }

}

