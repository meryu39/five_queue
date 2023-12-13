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


    private string[] skill_text = { "������ ���� ������ �ߵ��Ǿ�, ���ظ� 80%�� ����",
                                   "��ȭ��� �Ѹ� ���� ������ �⺻���� ��, ���鿡�� ��ȭ�� �л��� ���ظ� �߰��� ����", 
                                   "�� �� ũ�� ȸ���ϸ� ������ �ֵѷ�, ����� ���鿡�� �⺻ ������ �� �� ���ظ� ����",
                                   "ü���� ���������� 25%��ŭ ȸ����",
                                   "������ ��󿡰� �⺻ ���ݿ� �ش��ϴ� ���ظ� �ڵ����� ���� �ݰ���",
                                   "���濡 �˱⸦ ����, �⺻ ���� 5�� ��Ÿ� �� ���鿡�� ���� ���·� �⺻ ���� ���� ����",
                                   "������ ���ϰ� ������ �������, ���� ���� �� ���鿡�� �⺻ ������ 6�� ���ظ� ����." };


    public bool[] buttonClicked = new bool[7];


    public Sprite e_skill;
    public Sprite shift_skill;
    public Sprite get_skill;

    private State state;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player"); // �÷��̾� �±׸� ����Ͽ� ã��
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


    bool exp_learn(int learn_cost)
    {
        if(state.skill_exp >= learn_cost)
        {
            state.skill_exp -= learn_cost;
            return true;
        }
        else
        {
            return false;
            Debug.Log("exp�� �����ؼ� �����");
        }
    }
    public void OnButtonClick(int index)
    {
        bool canLearn;
        Debug.Log("��ư���� �ö�����");
        if (!buttonClicked[index])
        {
            
            SoundManager.instance.PlaySfx(SoundManager.Sfx.MiniButton);
            // Ŭ���� ��ư�� �̹��� ���� �� �÷��� ����
            switch (index)
            {
                case 0:
                    canLearn = exp_learn(3);
                    if (!canLearn) break;
                    button1.image.sprite = get_skill;
                    buttonClicked[0] = true;
                    break;
                case 1:
                    canLearn = exp_learn(3);
                    if (!canLearn) break;
                    button2.image.sprite = get_skill;
                    buttonClicked[1] = true;
                    break;
                case 2:
                    canLearn = exp_learn(3);
                    if (!canLearn) break;
                    button3.image.sprite = get_skill;
                    buttonClicked[2] = true;
                    break;
                case 3:
                    canLearn = exp_learn(3);
                    if (!canLearn) break;
                    button4.image.sprite = get_skill;
                    buttonClicked[3] = true;
                    break;
                case 4:
                    canLearn = exp_learn(3);
                    if (!canLearn) break;
                    button5.image.sprite = get_skill;
                    buttonClicked[4] = true;
                    break;
                case 5:
                    canLearn = exp_learn(2);
                    if (!canLearn) break;
                    button6.image.sprite = get_skill;
                    buttonClicked[5] = true;
                    break;
                case 6:
                    canLearn = exp_learn(4);
                    if (!canLearn) break;
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
            SoundManager.instance.PlaySfx(SoundManager.Sfx.MiniButton);
            // Ŭ���� ��ư�� �̹��� ���� �� �÷��� ����
            if (Input.GetMouseButtonDown(1)) 
            {
                if (state.active_e != -1)
                {
                    Debug.Log("e�����");
                    ChangeSprite(state.active_e, get_skill);

                }
                Debug.Log("e��ų");
                ChangeSprite(index, e_skill);

                state.active_e = index;
            }
            // �� �ܿ��� ������ ���콺 ��ư Ŭ�� (���� ����)
            else if (Input.GetMouseButtonDown(0))  
            {
                if (state.active_shift != -1)
                {
                    Debug.Log("����Ʈ�����");
                    ChangeSprite(state.active_shift, get_skill);

                }
                Debug.Log("����Ʈ��ų");
                ChangeSprite(index, shift_skill);

                state.active_shift = index;
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

