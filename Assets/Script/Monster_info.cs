using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Playables;
using Cinemachine;

public class Monster_info : MonoBehaviour
{

    public enum MonsterType
    {
        human,
        runner,
        heavy,
        trap
    }


    public MonsterType monsterType;
    [SerializeField]
    public float MonsterAttack { get { return GetMonsterAttack(); } }
    [SerializeField]
    public float moveSpeed { get { return GetMoveSpeed(); } }
    private bool runner_start = false;

    private State state;
    private PlayerMove dashing;

    private float attackCoolTime = 2f; // ���� ��Ÿ��
    private bool isAttacking = false; //���ݿ���
    private float lastAttackTime = 0f; //������ ���� 


    Transform playerTransform;
    bool isfollow = false;
    public float Monster_HP = 100;
    public GameObject M_area;
    public GameObject hpBarPrefab; // HP �� ������
    private Slider Monster_hpbar; // ���� ü�¹� 
    private GameObject Monster_area;
    public GameObject UIparent;


    [HideInInspector]
    public Animator my_anim;
    private Rigidbody2D Rb; //������ٵ�2d�� Rb ����

    private bool isFacingRight = true;

    SpriteRenderer spriteRenderer;


    private bool search = true;
    public float moveRange = 5f; // ������ �̵� ����


    float nextTime = 0f; // �����̵����� �ð�

    float moveDuration = 2f; // �̵��ϴ� �ð�
    bool isMoving = false; // ���� �̵� ������ ����

    private bool EnterPlayer = false;

    public GameObject boob;
    public GameObject trap_mouse;

    public GameObject blood_prefab;
    Transform bloodPosition;
    private void Awake()
    {
        state = FindObjectOfType<State>();
        dashing = FindObjectOfType<PlayerMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        my_anim = GetComponent<Animator>(); // �ִϸ����� ������Ʈ
        Rb = GetComponent<Rigidbody2D>(); // ������ٵ� ������Ʈ
        Rb.velocity = Vector3.zero;
        bloodPosition = dashing.transform.Find("blood_position");
    }

    void Start()
    {
        if (hpBarPrefab != null)
        {
            Monster_hpbar = Instantiate(hpBarPrefab, Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.05f, 0.3f, 0)), Quaternion.identity).GetComponent<Slider>();
            Monster_hpbar.transform.SetParent(UIparent.transform);
            Monster_hpbar.gameObject.SetActive(false);

            Monster_area = Instantiate(M_area, transform.position, Quaternion.identity);
            Monster_area.transform.parent = transform;
            Monster_area.gameObject.SetActive(true);
        }
        
        my_anim.speed = 0.5f;

        GetMonsterAttack();
        GetMoveSpeed();



    }

    private void Update()
    {
  
            if (isfollow && playerTransform != null)
            {

                if (!EnterPlayer &&  monsterType != MonsterType.trap){                
                    my_anim.SetBool("isRun", true);
                }

                Vector2 direction = (playerTransform.position - transform.position).normalized;

                // Flip ����
                if ((isFacingRight && direction.x < 0) || (!isFacingRight && direction.x > 0))
                {
                    Flip();
                }
                if (monsterType != MonsterType.trap)
                {
                    transform.Translate(direction * moveSpeed  * Time.fixedDeltaTime);
                }
            }

        
        if (Monster_hpbar != null)
        {
            Monster_hpbar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.05f, 0.8f, 0));
        }

        UpdateHP();



    }

    private void FixedUpdate()
    {
        if (monsterType != MonsterType.trap)
        {
            FSM_huamn();
        }

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
            my_anim.SetTrigger("isDead");
            Invoke("delete_monster", 0.5f);

        }
    }

    private void delete_monster()
    {
        Destroy(Monster_hpbar.gameObject);
        Destroy(gameObject);
    }



    
    private float GetMonsterAttack()
    {
        switch (monsterType)
        {
            case MonsterType.human:
                return 10f;
            case MonsterType.runner:
                return 20f;
            case MonsterType.heavy:
                return 40f;
            case MonsterType.trap:
                return 0f;
            default:
                return 20f; 
        }
    }

    //�÷��̾� �ӵ� 
    private float GetMoveSpeed()
    {
        switch (monsterType)
        {
            case MonsterType.human:
                return 1.5f;
            case MonsterType.runner:
                return 1.2f;
            case MonsterType.heavy:
                return 0.5f;
            case MonsterType.trap:
                return 0;
            default:
                return 0;        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            if (monsterType == MonsterType.runner && !runner_start)
            {
                my_anim.SetTrigger("isRush");
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                Rb.velocity = directionToPlayer * moveSpeed * 2f;
                runner_start = false;
            }

            isfollow = true;


            if (monsterType != MonsterType.trap)
            {
                Monster_area.SetActive(false);
            }

            Monster_hpbar.gameObject.SetActive(true);
            search = false;

        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && monsterType == MonsterType.trap)
        {
            Debug.Log("���� ���� ���ñ� ��");
            if (Time.time - lastAttackTime >= attackCoolTime)
            {
                boobing();
                lastAttackTime = Time.time;
            }
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            isAttacking = true;
            my_anim.SetTrigger("isAttack");
            state.SetHP(state.currentHP-MonsterAttack);
            lastAttackTime = Time.time;
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && monsterType != MonsterType.trap)
        {

            Rb.constraints = RigidbodyConstraints2D.FreezeAll;
            my_anim.SetBool("isRun", false);
            EnterPlayer = true;

            if (Time.time - lastAttackTime >= attackCoolTime)
            {
                my_anim.SetTrigger("isAttack");
                lastAttackTime = Time.time;

                if (!dashing.nodeal)
                {
                    state.SetHP(state.currentHP - MonsterAttack);
                    
                    SpawnBloodEffect(bloodPosition.position,dashing.transform);



                }


            }

        }



    }
    private void SpawnBloodEffect(Vector3 position,Transform parent)
    {
        GameObject bloodEffect = Instantiate(blood_prefab, position, Quaternion.identity);
        bloodEffect.transform.parent = parent;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        my_anim.SetBool("isRun", true);
        //Rb.constraints = RigidbodyConstraints2D.None;
        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;


    }


    void boobing()
    {
        float SkillSpeed = 3f;
        Vector2 skillDirection = (dashing.transform.position - transform.position).normalized;

        GameObject skill1 = Instantiate(boob, trap_mouse.transform.position, Quaternion.identity);
        Rigidbody2D skill1_rb = skill1.GetComponent<Rigidbody2D>();
        skill1_rb.velocity = skillDirection * SkillSpeed;

        // �÷��̾� ���⿡ ���� ��ų �������� ���� ��ȯ
        float angle = Mathf.Atan2(skillDirection.y, skillDirection.x) * Mathf.Rad2Deg;
        skill1.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        Destroy(skill1, 4f);

    }



    void FSM_huamn()
    {
        if (search && monsterType != MonsterType.trap)
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


                    nextTime = Random.Range(1f, 3f); // 1�ʿ��� 3�� ������ ������ �ð� ����
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


                    nextTime = Random.Range(1f, 3f); // 1�ʿ��� 3�� ������ ������ �ð� ����
                }
            }
        }
    }




}


