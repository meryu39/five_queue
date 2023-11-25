using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{

    public GameObject elev1;
    public GameObject elev2;


    private State state;
    private Monster_info monster;
    //private Skill skill;
    private bool dashpress = false;


    public UI_CoolTime coolTimeUI; // UI_CoolTime 클래스의 인스턴스에 접근하기 위한 변수



    private Rigidbody2D playerRb; //리자드바디2d를 playerRb로 선언
    private Animator myAnim; 
    public float playerMoveSpeed;
    
    private bool isRun; //달리기 여부 
    
    private bool canDash = true; //대쉬가능여부
    private bool isDashing; //대쉬여부
    private float dashingPower = 10f; //대쉬이동거리
    private float dashingTime = 0.2f; //대쉬이동시간
    private float dashingCooldown = 2f; //대쉬 쿨타임
    
    private bool isAttack = false; //공격확인 플래그(중복 공격 방지)
    private bool Attacking = false;  //공격키 입력 확인 변수


    int skillnum;
    float skillCoolTIme = 3f;
    float lastskillTime = 0;

    [SerializeField] private TrailRenderer tr;
    
    public  bool nodeal = false; //무!!!적!!!!!!!!판!!!정!~!!!기  <- 미쳐버린 서경식
    [SerializeField]
    public GameObject 혈취처방;
    //상호작용과 관련된 변수들
    private GameObject interactionObject;   //상호작용하는 오브젝트
    private int interactionObjectCount;     //상호작용 범위에 들어온 오브젝트 개수, 상호작용 범위 안에 여러 개의 상호작용 오브젝트가 있는 경우를 처리하기 위함
    public GameObject Image_PressF;         //F키를 눌러달라는 UI 오브젝트
    //아이템 버리기와 관련된 변수들
    private bool[] isDump = { false, false, false };    //현재 버리기 버튼을 누르고 있는지 확인하는 변수
    public const float dumpTime = 1.25f;                      //버리기까지 버튼을 눌러야 하는 시간
    //아이템 회복량과 관련된 변수들
    private const float bandage_HPRecoveryPercent = 0.1f;
    private const float painkiller_energyRecoveryPercent = 0.5f;
    private const float epinephrine_energyRecoveryPercent = 1.0f;
    private const int can_hungerRecoveryAmount = 1;
    private const int cupramen_hungerRecoveryAmount = 2;

    



    private void Awake()
    {
        
        playerRb = GetComponent<Rigidbody2D>(); //리자드바디 컴포넌트
        playerRb.velocity = Vector3.zero;
        myAnim = GetComponent<Animator>(); //애니메이터 컴포넌트
        state = GetComponent<State>(); // 스탯 스크립트 연결
        //skill = GetComponent<Skill>();
        GameObject monsterObject = GameObject.FindWithTag("Monster"); // 몬스터의 태그를 사용하여 찾음
        if(monsterObject != null)
        {
            //Debug.Log("몬스터태그찾음");
        }
        Monster_info monster = monsterObject.GetComponent<Monster_info>();
        if(monster != null)
        {
            //Debug.Log("몬스터컴포넌트 됨");
        }
        Image_PressF.SetActive(false);      //'F키를 누르시오' UI 비활성화 시켜놓기

    }
    private void Start()
    {
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        checkInput();       //입력한 버튼이 있는지 확인
                            //객체끼리 충돌시 밀리지 않기 가속도 = 0
                            // GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
    }
    private void checkInput()       //입력한 버튼이 있는지 확인하는 함수
    {
        //마우스 좌클릭 + Attack 애니메이션이 진행중이지 않을 때,
        if ((Input.GetMouseButtonDown(0)) && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Attacking = true;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetPlayerDirection(mousePos - new Vector2(transform.position.x, transform.position.y));


        }


            //F키를 누르면 상호작용
            if (Input.GetKeyDown(KeyCode.F))
        {
            if (interactionObjectCount > 0)
            {
                InteractionManager.instance.Interact(interactionObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && (canDash) && !isDashing)
        {
            StartCoroutine(Dash());  // 대쉬 코루틴 실행
        }
        //숫자 1, 2, 3번을 누르면 각각 아이템의 버리기 카운팅을 시작한다. dumpTime동안 떼지 않으면 아이템을 버린다.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isDump[0] = true;
            StartCoroutine(DumpItem(0));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isDump[1] = true;
            StartCoroutine(DumpItem(1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isDump[2] = true;
            StartCoroutine(DumpItem(2));
        }
        //숫자 1, 2, 3번을 떼면 버리기 카운팅을 중단하고 해당 아이템을 사용한다.
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            isDump[0] = false;
            UseItem(0);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            isDump[1] = false;
            UseItem(1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            isDump[2] = false;
            UseItem(2);
        }
        if(Input.GetMouseButtonDown(1))/*우클릭*/
        {
            UseWeapon();
        }
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
    
        playerRb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * playerMoveSpeed * Time.fixedDeltaTime; //사용자 방향키 입력받아 이동속도 계산
                                                                                                                                                          //Debug.Log(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * playerMoveSpeed * Time.fixedDeltaTime);



        myAnim.SetFloat("MoveX", playerRb.velocity.x);             //파라미터 선언
        myAnim.SetFloat("MoveY", playerRb.velocity.y);               


        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            myAnim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));                    //마지막으로 이동한 방향 확인하기 위한 파라미터 선언
            myAnim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
        }
    
        if (dashpress && canDash && !isDashing)
        {
            StartCoroutine(Dash());
            dashpress = false; // 대쉬 입력을 처리한 후에는 리셋
        }
        if (Attacking)
        {
            myAnim.SetTrigger("isAttack"); //공격애니메이션 실행
            isAttack = false; //공격플래그 비활성화
            Attacking = false; //공격모션 비활성화
        }
    
        if (Input.GetKey(KeyCode.LeftShift) && !isDashing) // 왼쪽 Shift 키를 누르고 대쉬 중이 아닌 경우
        {
            isRun = true; // 달리기 상태로 설정
            playerMoveSpeed = 500f; //달리기 20 증가  150
            myAnim.SetBool("isRun", true);
        }
        else
        {
            isRun = false; // 달리기 x
            playerMoveSpeed = 100f; // 달리지않을 때, 원래 속도로 100
            myAnim.SetBool("isRun", false);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (Time.time - lastskillTime >= skillCoolTIme)
            {

                SkillEvenet(1);
                lastskillTime = Time.time;
            }
        }


     

    }



    private void SetPlayerDirection(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return;
        direction.Normalize();
        myAnim.SetFloat("AttackX", direction.x);
        myAnim.SetFloat("AttackY", direction.y);
        myAnim.SetFloat("LastMoveX", direction.x);
        myAnim.SetFloat("LastMoveY", direction.y);

    }

    private IEnumerator Dash()
    {


        if (coolTimeUI != null)
        {
            coolTimeUI.Trigger_Skill();
        }
        canDash = false;
      
        isDashing = true;
        nodeal = true;
    
        float originalGravity = playerRb.gravityScale; //중력 수치 
        playerRb.gravityScale = 0f; //중력 0으로 만듬 


        float dashDirectionX = Input.GetAxisRaw("Horizontal");
        float dashDirectionY = Input.GetAxisRaw("Vertical");
    
        Vector2 dashDirection = new Vector2(dashDirectionX, dashDirectionY).normalized;
        playerRb.velocity = dashDirection * dashingPower;
        
        tr.emitting = true;


        yield return new WaitForSeconds(dashingTime); //해당 변수만큼 기다림
        tr.emitting = false; //이펙트 종료 
        playerRb.gravityScale = originalGravity; 
        isDashing = false;
        nodeal = false;
    
        yield return new WaitForSeconds(dashingCooldown);  //해당 변수만큼 기다림        
        canDash = true;


    }


    //스킬1
    void SkillEvenet(int skillnum)
    {
        float SkillSpeed = 3f;
        if(skillnum == 1) { 
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SetPlayerDirection(mousePosition - new Vector2(transform.position.x, transform.position.y));
                myAnim.SetTrigger("isAttack");

                Vector2 skillDirection = (mousePosition - new Vector2(transform.position.x, transform.position.y)).normalized;
                skillDirection.y = 0;

                GameObject skill1 = Instantiate(혈취처방, transform.position, Quaternion.identity);
                Rigidbody2D skill1_rb = skill1.GetComponent<Rigidbody2D>();
                skill1_rb.velocity = skillDirection * SkillSpeed;

                //Atan2는 y,x좌표로 두 점 사이의 각도 계산 (플레이어 방향에 따른 프리팹의 방향전환 -> 스킬프리팹 방향)
                float angle = Mathf.Atan2(skillDirection.y, skillDirection.x) * Mathf.Rad2Deg;
                //스킬 프리팹 방향 전환
                skill1.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                Destroy(skill1, 4f);


        }
    }

    private Vector2 GetPlayerDirection()
    {
        //플레이어 방향 리턴
        return new Vector2(myAnim.GetFloat("LastMoveX"), myAnim.GetFloat("LastMoveY")).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌된 태그가 몬스터이고, 공격 모션이 실행되지 않았을 때, 공격 플래그가 활성화되지 않았을 때
        if (other.CompareTag("Monster") && !isAttack)
        {
            Debug.Log("몬스터가 닿았습니다");
            // 태그된 객체의 몬스터 인포 컴포넌트를 가져옴
            Monster_info monster = other.GetComponent<Monster_info>();
    
            if (monster != null)
            {
                Debug.Log("공격 성공");
                monster.my_anim.SetTrigger("isHurt");
                // 몬스터 스크립트에 몬스터 체력에 state 스크립트의 공격값을 뺌
                monster.Monster_HP -= state.PlayerAttackDamage;
                // 공격 플래그 활성화
                isAttack = true;
            }
        }
       
    
        //InteractionObject 범위 안에 들어간 경우
        if(other.CompareTag("InteractionObject"))
        {
            //아직 상호작용 범위 안에 들어간 오브젝트가 없었던 경우에만 pressF UI 출력
            if(interactionObjectCount == 0)
            {
                Image_PressF.SetActive(true);
            }
            interactionObjectCount++;
            interactionObject = other.gameObject;   //InteractionObject를 현재 접촉한 오브젝트로 변경
        }
        if (other.gameObject == elev1)
        {
            Vector3 newPosition = new Vector3(-84, -71, 0);
            transform.position = newPosition;

        }
        else if (other.gameObject == elev2)
        {
            Vector3 newPosition = new Vector3(164, -17, 0);
            transform.position = newPosition;

        }
    }
    
    
    private void OnTriggerExit2D(Collider2D other)
    {
        //상호작용 오브젝트의 상호작용 범위 밖으로 나갈 경우 pressF UI를 닫는다.
        if(other.CompareTag("InteractionObject"))
        {
            interactionObjectCount--;
            if(interactionObjectCount == 0)
            {
                //Debug.Log("패널 닫음");
                Image_PressF.SetActive(false);
            }
        }
    }
    
    private void UseItem(int itemIndex)     //아이템을 사용하는 함수이다. 아이템 종류에 따른 기능을 수행한다.
    {
        ref Item usingItem = ref state.item[itemIndex];
        //사용하려는 아이템의 개수가 없는 경우
        if(usingItem.count <= 0)
        {
            Debug.Log(itemIndex + 1 + "번째 아이템 남은 개수가 없음");
            return;
        }
        usingItem.count--;
        if(usingItem.name == InteractionObjectName.BANDAGE)  //붕대
        {
            state.SetHP(state.currentHP + state.maxHP * bandage_HPRecoveryPercent);
        }
        else if (usingItem.name == InteractionObjectName.PAINKILLER) //진통제
        {
            state.SetEnergy(state.currentEnergy + state.maxEnergy * painkiller_energyRecoveryPercent);
        }
        else if (usingItem.name == InteractionObjectName.EPINEPHRINE)    //에피네프린
        {
            state.SetEnergy(state.currentEnergy + state.maxEnergy * epinephrine_energyRecoveryPercent);
        }
        else if (usingItem.name == InteractionObjectName.CAN)    //통조림
        {
            state.SetHunger(state.currentHunger + can_hungerRecoveryAmount);
        }
        else if (usingItem.name == InteractionObjectName.CUPRAMEN)   //컵라면
        {
            state.SetHunger(state.currentHunger + cupramen_hungerRecoveryAmount);
        }
    }

    private void UseWeapon()
    {
        ref Item usingWeapon = ref state.auxiliaryWeapon;
        if(usingWeapon.count <= 0)
        {
            return;
        }
        usingWeapon.count--;
    }
    
    private IEnumerator DumpItem(int itemIndex)     //아이템 버리는 시간을 카운팅하는 코루틴 함수이다.
    {
        yield return new WaitForSeconds(dumpTime);
        if (isDump[itemIndex] == true)
        {
            state.item[itemIndex].count = 0;
        }
    }

    
}