using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Playables;
using Cinemachine;

public class Monster_info : MonoBehaviour
{

    private State state;
    private PlayerMove dashing;



    private float attackCoolTime = 3f; // ���� ��Ÿ��
    private bool isAttacking = false; //���ݿ���
    private float lastAttackTime = 0f; //������ ���� 



    public float MonsterAttack = 20f;
    public float moveSpeed = 1f;
    Transform playerTransform;
    bool isfollow = false;
    public float Monster_HP = 100;
    public GameObject M_area;
    public GameObject hpBarPrefab; // HP �� ������
    private Slider Monster_hpbar; // ���� ü�¹� 
    private GameObject Monster_area;
    public GameObject UIparent;

    private Animator my_anim;
    private Rigidbody2D Rb; //������ٵ�2d�� Rb ����

    private bool isFacingRight = true;

    SpriteRenderer spriteRenderer;


    private bool search = true;
    public float moveRange = 5f; // ������ �̵� ����


    float nextTime = 0f; // �����̵����� �ð�

    float moveDuration = 2f; // �̵��ϴ� �ð�
    bool isMoving = false; // ���� �̵� ������ ����

    private bool EnterPlayer = false; 
    private void Awake()
    {
        state = FindObjectOfType<State>();
        dashing = FindObjectOfType<PlayerMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        my_anim = GetComponent<Animator>(); // �ִϸ����� ������Ʈ
        Rb = GetComponent<Rigidbody2D>(); // ������ٵ� ������Ʈ
        Rb.velocity = Vector3.zero;
    }

    void Start()
    {
        if (hpBarPrefab != null)
        {
            Monster_hpbar = Instantiate(hpBarPrefab, Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.05f, 0.3f, 0)), Quaternion.identity).GetComponent<Slider>();
            Monster_hpbar.transform.parent = UIparent.transform;
            Monster_hpbar.gameObject.SetActive(false);

            Monster_area = Instantiate(M_area, transform.position, Quaternion.identity);
            Monster_area.transform.parent = transform;
            Monster_area.gameObject.SetActive(true);
        }



    }

    private void Update()
    {
        if (isfollow && playerTransform != null)
        {

            if (!EnterPlayer)
            {
                my_anim.SetBool("isRun", true);
            }
            Vector2 direction = (playerTransform.position - transform.position).normalized;

            // Flip ����
            if ((isFacingRight && direction.x < 0) || (!isFacingRight && direction.x > 0))
            {
                Flip();
            }

            transform.Translate(direction * (moveSpeed + 1f) * Time.deltaTime);
        }

        if (Monster_hpbar != null)
        {
            Monster_hpbar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.05f, 0.3f, 0));
        }

        UpdateHP();



    }

    private void FixedUpdate()
    {
        FSM();
    }
    private void Flip()
    {
        // ���� ���¸� ����
        isFacingRight = !isFacingRight;

        // SpriteRenderer�� �̿��Ͽ� ��������Ʈ�� ������

        spriteRenderer.flipX = !isFacingRight;
    }

    private void UpdateHP()
    {
        if (Monster_hpbar != null)
        {
            Monster_hpbar.value = (float)Monster_HP / 100;
        }

        if (Monster_HP <= 0)
        {
            Destroy(Monster_hpbar.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            isfollow = true;
            Monster_area.SetActive(false);
            Monster_hpbar.gameObject.SetActive(true);
            search = false;

        }
    }





    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            isAttacking = true;
            my_anim.SetTrigger("isAttack");
            state.Pdamage(MonsterAttack);
            lastAttackTime = Time.time;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            my_anim.SetBool("isRun", false);
            EnterPlayer = true;

                if (Time.time - lastAttackTime >= attackCoolTime)
                {
                    my_anim.SetTrigger("isAttack");
                    lastAttackTime = Time.time;

                    if (!dashing.nodeal)
                    {
                        state.Pdamage(MonsterAttack);
                    }

                    
                }
            
        }
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        my_anim.SetBool("isRun", true);
    }




    /*private void OnCollisionExit2D(Collision2D collision)
    {
        if (isAttacking&&my_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            // �浹���� ����� �� ������ �����ϰ� isAttacking �缳��
            isAttacking = false;
        }
    }
    */

    void FSM()
    {
        if (search)
        {
            if (!isMoving)
            {
                nextTime -= Time.deltaTime;

                if (nextTime <= 0f)
                {
                    // ������ ���� ��ġ
                    Vector2 currentPosition = transform.position;

                    // ������ ������ �̵� ���� ����
                    Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    randomDirection.Normalize();

                    // ���Ͱ� �̵��� ���ο� ��ġ ���
                    Vector2 newPosition = currentPosition + randomDirection * moveRange;

                    // �̵���������
                    newPosition.x = Mathf.Clamp(newPosition.x, currentPosition.x - moveRange, currentPosition.x + moveRange);
                    newPosition.y = Mathf.Clamp(newPosition.y, currentPosition.y - moveRange, currentPosition.y + moveRange);

   
                    if (randomDirection != Vector2.zero)
                    {
                        //���������� 0�� �ƴϸ� ��������

                        my_anim.SetBool("isWalk", true);
                        isMoving = true;
                        Rb.velocity = randomDirection * moveSpeed;

                        //���� �޶��������� �ø��Ἥ ������ȯ
                        if (randomDirection.x > 0 && !isFacingRight)
                        {
                            Flip();
                        }
                        else if (randomDirection.x < 0 && isFacingRight)
                        {
                            Flip();
                        }
                    }
                    else
                    {

                        my_anim.SetBool("isWalk", false);
                        isMoving = false;
                    }


                    nextTime = moveDuration;
                }
            }
            else
            {
                nextTime -= Time.deltaTime;

                if (nextTime <= 0f)
                {
                    my_anim.SetBool("isWalk", false);
                    Rb.velocity = Vector2.zero;
                    isMoving = false;


                    nextTime = moveDuration;
                }
            }
        }
    }




}


