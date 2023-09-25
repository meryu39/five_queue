using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{

    public UI_CoolTime coolTimeUI; // UI_CoolTime Ŭ������ �ν��Ͻ��� �����ϱ� ���� ����



    private Rigidbody2D playerRb;
    private Animator myAnim;
    public float playerMoveSpeed;

    private bool isRun; //�޸��� ���� 

    private bool canDash = true; //�뽬���ɿ���
    private bool isDashing; //�뽬����
    private float dashingPower = 50; //�뽬�̵��Ÿ�
    private float dashingTime = 0.2f; //�뽬�̵��ð�
    private float dashingCooldown = 2f; //�뽬 ��Ÿ��

    [SerializeField] private TrailRenderer tr;


    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>(); //���ڵ�ٵ� ������Ʈ
        myAnim = GetComponent<Animator>(); //�ִϸ����� ������Ʈ
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        //Debug.Log("�뽬���ɿ��δ�" + canDash);
        //Debug.Log("���� �뽬���´�" + isDashing);

        if (Input.GetKeyDown(KeyCode.F) && (canDash))
        {
            StartCoroutine(Dash());  // �뽬 �ڷ�ƾ ����
        }
    }
    private void FixedUpdate()
    {

        playerRb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * playerMoveSpeed * Time.fixedDeltaTime; //����� ����Ű �Է¹޾� �̵��ӵ� ���
        //Debug.Log(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * playerMoveSpeed * Time.fixedDeltaTime);

        myAnim.SetFloat("MoveX", playerRb.velocity.x);             //�Ķ���� ����
        myAnim.SetFloat("MoveY", playerRb.velocity.y);               


        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            myAnim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));                    //���������� �̵��� ���� Ȯ���ϱ� ���� �Ķ���� ����
            myAnim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
        }
        if (Input.GetKeyDown(KeyCode.F) && (canDash))   //���� ����ƮŰ + �뽬���ɿ���
        {
            StartCoroutine(Dash());            //�뽬 �ڷ�ƾ ����
        }

        if (isDashing)
        {
            return;
        }
        //AŰ�� ������ �� + Attack �ִϸ��̼��� ���������� ���� ��,
        if ((Input.GetMouseButtonDown(0)) && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            myAnim.SetTrigger("isAttack"); //���ݾִϸ��̼� ����
        }

        if (Input.GetKey(KeyCode.LeftShift) && !isDashing) // ���� Shift Ű�� ������ �뽬 ���� �ƴ� ���
        {
            isRun = true; // �޸��� ���·� ����
            playerMoveSpeed = 50f; //�޸��� 20 ���� 
            myAnim.SetBool("isRun", true);
        }
        else
        {
            isRun = false; // �޸��� x
            playerMoveSpeed = 30f; // �޸������� ��, ���� �ӵ���
            myAnim.SetBool("isRun", false);

        }

    }

    private IEnumerator Dash()
    {
        Debug.Log("�뽬��ư����");

        if (coolTimeUI != null)
        {
            coolTimeUI.Trigger_Skill();
        }
        canDash = false;
        Debug.Log("��ư�� ������ canDash�� false��");
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
        Debug.Log("�ð��� ������ canDash�� true");

    }
}