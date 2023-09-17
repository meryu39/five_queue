using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Animator myAnim;
    public float playerMoveSpeed;


    private bool canDash = true; //�뽬���ɿ���
    private bool isDashing; //�뽬����
    private float dashingPower = 50; //�뽬�̵��Ÿ�
    private float dashingTime = 0.2f; //�뽬�̵��ð�
    private float dashingCooldown = 1f; //�뽬 ��Ÿ��

    [SerializeField] private TrailRenderer tr;


    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>(); //���ڵ�ٵ� ������Ʈ
        myAnim = GetComponent<Animator>(); //�ִϸ����� ������Ʈ
    }

    private void Update()
    {
        if (isDashing)             //�뽬���θ���
        {
            return;
        }

    }

    private void FixedUpdate()
    {


        playerRb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * playerMoveSpeed * Time.deltaTime; //����� ����Ű �Է¹޾� �̵��ӵ� ���

        myAnim.SetFloat("MoveX", playerRb.velocity.x);             //�Ķ���� ����
        myAnim.SetFloat("MoveY", playerRb.velocity.y);               


        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            myAnim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));                    //���������� �̵��� ���� Ȯ���ϱ� ���� �Ķ���� ����
            myAnim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && (canDash))   //���� ����ƮŰ + �뽬���ɿ���
        {
            StartCoroutine(Dash());            //�뽬 �ڷ�ƾ ����
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
        float originalGravity = playerRb.gravityScale; //�߷� ��ġ 
        playerRb.gravityScale = 0f; //�߷� 0���� ���� 


        float dashDirectionX = Input.GetAxisRaw("Horizontal");
        float dashDirectionY = Input.GetAxisRaw("Vertical");


        playerRb.velocity = new Vector2(dashDirectionX * dashingPower, dashDirectionY * dashingPower); //x,y�� ���� �뽬�Ŀ��� ���
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime); //�ش� ������ŭ ��ٸ�
        tr.emitting = false; //����Ʈ ���� 
        playerRb.gravityScale = originalGravity; 
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);  //�ش� ������ŭ ��ٸ�
        canDash = true; 
    }
}