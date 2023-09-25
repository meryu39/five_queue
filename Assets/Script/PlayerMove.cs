using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{

    public UI_CoolTime coolTimeUI; // UI_CoolTime 클래스의 인스턴스에 접근하기 위한 변수



    private Rigidbody2D playerRb;
    private Animator myAnim;
    public float playerMoveSpeed;

    private bool isRun; //달리기 여부 

    private bool canDash = true; //대쉬가능여부
    private bool isDashing; //대쉬여부
    private float dashingPower = 50; //대쉬이동거리
    private float dashingTime = 0.2f; //대쉬이동시간
    private float dashingCooldown = 2f; //대쉬 쿨타임

    [SerializeField] private TrailRenderer tr;


    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>(); //리자드바디 컴포넌트
        myAnim = GetComponent<Animator>(); //애니메이터 컴포넌트
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        //Debug.Log("대쉬가능여부는" + canDash);
        //Debug.Log("현재 대쉬상태는" + isDashing);

        if (Input.GetKeyDown(KeyCode.F) && (canDash))
        {
            StartCoroutine(Dash());  // 대쉬 코루틴 실행
        }
    }
    private void FixedUpdate()
    {

        playerRb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * playerMoveSpeed * Time.fixedDeltaTime; //사용자 방향키 입력받아 이동속도 계산
        //Debug.Log(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * playerMoveSpeed * Time.fixedDeltaTime);

        myAnim.SetFloat("MoveX", playerRb.velocity.x);             //파라미터 선언
        myAnim.SetFloat("MoveY", playerRb.velocity.y);               


        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            myAnim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));                    //마지막으로 이동한 방향 확인하기 위한 파라미터 선언
            myAnim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
        }
        if (Input.GetKeyDown(KeyCode.F) && (canDash))   //왼쪽 쉬프트키 + 대쉬가능여부
        {
            StartCoroutine(Dash());            //대쉬 코루틴 실행
        }

        if (isDashing)
        {
            return;
        }
        //A키를 눌렀을 때 + Attack 애니메이션이 진행중이지 않을 때,
        if ((Input.GetMouseButtonDown(0)) && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            myAnim.SetTrigger("isAttack"); //공격애니메이션 실행
        }

        if (Input.GetKey(KeyCode.LeftShift) && !isDashing) // 왼쪽 Shift 키를 누르고 대쉬 중이 아닌 경우
        {
            isRun = true; // 달리기 상태로 설정
            playerMoveSpeed = 50f; //달리기 20 증가 
            myAnim.SetBool("isRun", true);
        }
        else
        {
            isRun = false; // 달리기 x
            playerMoveSpeed = 30f; // 달리지않을 때, 원래 속도로
            myAnim.SetBool("isRun", false);

        }

    }

    private IEnumerator Dash()
    {
        Debug.Log("대쉬버튼눌림");

        if (coolTimeUI != null)
        {
            coolTimeUI.Trigger_Skill();
        }
        canDash = false;
        Debug.Log("버튼이 눌려서 canDash가 false됨");
        isDashing = true;
        float originalGravity = playerRb.gravityScale; //중력 수치 
        playerRb.gravityScale = 0f; //중력 0으로 만듬 


        float dashDirectionX = Input.GetAxisRaw("Horizontal");
        float dashDirectionY = Input.GetAxisRaw("Vertical");


        playerRb.velocity = new Vector2(dashDirectionX * dashingPower, dashDirectionY * dashingPower); //x,y축 각각 대쉬파워랑 계산
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime); //해당 변수만큼 기다림
        tr.emitting = false; //이펙트 종료 
        playerRb.gravityScale = originalGravity; 
        isDashing = false;
        
        yield return new WaitForSeconds(dashingCooldown);  //해당 변수만큼 기다림
        canDash = true;
        Debug.Log("시간이 지나서 canDash가 true");

    }
}