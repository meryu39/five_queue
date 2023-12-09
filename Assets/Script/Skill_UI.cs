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


    public string[] skill_text = { "[신경퇴화]", "[독성소독]", "[집단 복부 절개]", "[심호흡]",
                                   "[과민반응]", "[혈취 처방]", "[도끼 광선]" };


    public bool[] buttonClicked = new bool[7];

    public int active_e = -1;
    public int active_shift = -1;

    public Sprite e_skill;
    public Sprite shift_skill;
    public Sprite get_skill;

    private State state;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player"); // 몬스터의 태그를 사용하여 찾음
        state = player.GetComponent<State>();


    }
    private void Start()
    {
        Debug.Log("Skill_UI 스크립트 시작됨");

        // 각 버튼에 이벤트 트리거 추가
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
        // 버튼에 클릭 이벤트 추가
        button.onClick.AddListener(() => OnButtonClick(index));

        // 마우스가 버튼 위에 올라갈 때 이벤트 추가
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
            Debug.Log("exp가 부족해서 못배움");
        }
    }
    public void OnButtonClick(int index)
    {

        if (!buttonClicked[index])
        {
            // 클릭된 버튼의 이미지 변경 및 플래그 설정
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
            // 클릭된 버튼의 이미지 변경 및 플래그 설정
            if (Input.GetMouseButtonDown(1)) 
            {
                if (active_e != -1)
                {
                    Debug.Log("e변경완");
                    ChangeSprite(active_e, get_skill);

                }
                Debug.Log("e스킬");
                ChangeSprite(index, e_skill);

                active_e = index;
            }
            // 그 외에는 오른쪽 마우스 버튼 클릭 (단축 누름)
            else if (Input.GetMouseButtonDown(0))  
            {
                if (active_shift != -1)
                {
                    Debug.Log("쉬프트변경완");
                    ChangeSprite(active_shift, get_skill);

                }
                Debug.Log("쉬프트스킬");
                ChangeSprite(index, shift_skill);

                active_shift = index;
            }
        }
    }




    private void ChangeSprite(int index, Sprite newSprite)
    {
        // 스프라이트 변경
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
        // 텍스트 출력
        skilltext.text = skill_text[index];
    }

    public void Print_text_NULL()
    {
        // 텍스트 공백으로 설정
        skilltext.text = " ";
    }

}

