using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Animator myAnim;
    public float playerMoveSpeed;


    private bool canDash = true; //대쉬가능여부
    private bool isDashing; //대쉬여부
    private float dashingPower = 50; //대쉬이동거리
    private float dashingTime = 0.2f; //대쉬이동시간
    private float dashingCooldown = 1f; //대쉬 쿨타임

    [SerializeField] private TrailRenderer tr;


    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>(); //리자드바디 컴포넌트
        myAnim = GetComponent<Animator>(); //애니메이터 컴포넌트
    }

    private void Update()
    {
        if (isDashing)             //대쉬여부리턴
        {
            return;
        }

    }

    private void FixedUpdate()
    {


        playerRb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * playerMoveSpeed * Time.deltaTime; //사용자 방향키 입력받아 이동속도 계산

        myAnim.SetFloat("MoveX", playerRb.velocity.x);             //파라미터 선언
        myAnim.SetFloat("MoveY", playerRb.velocity.y);               


        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            myAnim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));                    //마지막으로 이동한 방향 확인하기 위한 파라미터 선언
            myAnim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && (canDash))   //왼쪽 쉬프트키 + 대쉬가능여부
        {
            StartCoroutine(Dash());            //대쉬 코루틴 실행
        }

        if (isDashing)
        {
            return;
        }

    }

    private IEnumerator Dash()
    {
        
        canDash = false;
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
    }
}