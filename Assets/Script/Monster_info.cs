using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Monster_info : MonoBehaviour
{

    
    float MonsterAttack = 5f;
    public float moveSpeed = 1f;
    Transform playerTransform;
    bool isfollow = false;
    public float Monster_HP = 100;
    public GameObject M_area;
    public GameObject hpBarPrefab; // HP 바 프리팹
    private Slider Monster_hpbar; // 몬스터 체력바 
    private GameObject Monster_area;
    public GameObject UIparent;

    private Animator my_anim;
    private Rigidbody2D Rb; //리지드바디2d를 Rb 선언

    private bool isFacingRight = true;

    SpriteRenderer spriteRenderer;


    private bool search = true;
    public float moveRange = 5f; // 몬스터의 이동 범위


    float nextTime = 0f; // 다음이동까지 시간

    float moveDuration = 2f; // 이동하는 시간
    bool isMoving = false; // 현재 이동 중인지 여부

    
    private void Awake()
    {
        State state = GameObject.Find("Player").GetComponent<State>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        my_anim = GetComponent<Animator>(); // 애니메이터 컴포넌트
        Rb = GetComponent<Rigidbody2D>(); // 리지드바디 컴포넌트
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

            my_anim.SetBool("isRun", true);
            Vector2 direction = (playerTransform.position - transform.position).normalized;

            // Flip 설정
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
        // 현재 상태를 반전
        isFacingRight = !isFacingRight;

        // SpriteRenderer를 이용하여 스프라이트를 뒤집음

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
        if (collision.gameObject.CompareTag("Player"))
        {
            my_anim.SetTrigger("isAttack");
        }
    }

    void FSM()
    {
        if (search)
        {
            if (!isMoving)
            {
                nextTime -= Time.deltaTime;

                if (nextTime <= 0f)
                {
                    // 몬스터의 현재 위치
                    Vector2 currentPosition = transform.position;

                    // 몬스터의 무작위 이동 벡터 생성
                    Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    randomDirection.Normalize();

                    // 몬스터가 이동할 새로운 위치 계산
                    Vector2 newPosition = currentPosition + randomDirection * moveRange;

                    // 이동범위제한
                    newPosition.x = Mathf.Clamp(newPosition.x, currentPosition.x - moveRange, currentPosition.x + moveRange);
                    newPosition.y = Mathf.Clamp(newPosition.y, currentPosition.y - moveRange, currentPosition.y + moveRange);

   
                    if (randomDirection != Vector2.zero)
                    {
                        //랜덤방향이 0이 아니면 수색진행

                        my_anim.SetBool("isWalk", true);
                        isMoving = true;
                        Rb.velocity = randomDirection * moveSpeed;

                        //방향 달라질때마다 플립써서 방향전환
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
