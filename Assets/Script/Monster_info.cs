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

    public float moveDistance = 3f; // 이동 거리
    public float changeDirectionInterval = 1.0f; // 방향 변경 간격
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 currentDirection;
    private bool isMoving = true;

    private float idleTime = 4f;
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

        SetMove();

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

        SelfMove();

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
            isMoving = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            my_anim.SetTrigger("isAttack");
        }
    }

    void SetMove()
    {
        startPosition = transform.position;
        currentDirection = RandomDirection();
        targetPosition = startPosition + currentDirection * moveDistance;
        StartCoroutine(ChangeDirection());
    }
    void SelfMove()
    {
        if (isMoving)
        {
            my_anim.SetBool("isWalk", true);
            transform.position += currentDirection * moveSpeed * Time.deltaTime;

            // 현재 위치와 목표 위치 사이의 거리를 계산
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            //Debug.Log(distanceToTarget);
            if (distanceToTarget < 0.1f)
            {

                Debug.Log(distanceToTarget);
                // 목표 위치에 도달하면 방향을 반대로 변경하고 목표 위치를 시작 위치로 설정
                currentDirection = -currentDirection;


                if (currentDirection == Vector3.left)
                {
                    targetPosition = new Vector3(startPosition.x - moveDistance, startPosition.y, startPosition.z);
                    Flip();
                    Debug.Log("Flip()실행");

                }
                else if (currentDirection == Vector3.right)
                {
                    targetPosition = new Vector3(startPosition.x + moveDistance, startPosition.y, startPosition.z);
                    Flip();
                    Debug.Log("Flip()실행");
                }
                else if (currentDirection == Vector3.up)
                {
                    targetPosition = new Vector3(startPosition.x, startPosition.y + moveDistance, startPosition.z);
                    Debug.Log("위로");
                }
                else if (currentDirection == Vector3.down)
                {
                    targetPosition = new Vector3(startPosition.x, startPosition.y - moveDistance, startPosition.z);
                }
            }
            //idleMove();

        }
    }

    IEnumerator idleMove()
    {
        my_anim.SetBool("isWalk", false);
        yield return new WaitForSeconds(idleTime);
        StartCoroutine(ChangeDirection());

    }
    IEnumerator ChangeDirection()
    {
        while (isMoving)
        {
            currentDirection = RandomDirection();
            yield return new WaitForSeconds(changeDirectionInterval);
        }
    }

    Vector3 RandomDirection()
    {
        float horizontal = Random.Range(-1f, 1f);
        float vertical = Random.Range(-1f, 1f);
        return new Vector3(horizontal, vertical, 0).normalized;
    }


}
