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
    public GameObject hpBarPrefab; // HP �� ������
    private Slider Monster_hpbar; // ���� ü�¹� 
    private GameObject Monster_area;
    public GameObject UIparent;

    private Animator my_anim;
    private Rigidbody2D Rb; //������ٵ�2d�� Rb ����

    private bool isFacingRight = true;

    SpriteRenderer spriteRenderer;

    public float moveDistance = 3f; // �̵� �Ÿ�
    public float changeDirectionInterval = 1.0f; // ���� ���� ����
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 currentDirection;
    private bool isMoving = true;

    private float idleTime = 4f;
    private void Awake()
    {
        State state = GameObject.Find("Player").GetComponent<State>();
        
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

        SetMove();

    }

    private void Update()
    {
        if (isfollow && playerTransform != null)
        {
            my_anim.SetBool("isRun", true);
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

        SelfMove();

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

            // ���� ��ġ�� ��ǥ ��ġ ������ �Ÿ��� ���
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            //Debug.Log(distanceToTarget);
            if (distanceToTarget < 0.1f)
            {

                Debug.Log(distanceToTarget);
                // ��ǥ ��ġ�� �����ϸ� ������ �ݴ�� �����ϰ� ��ǥ ��ġ�� ���� ��ġ�� ����
                currentDirection = -currentDirection;


                if (currentDirection == Vector3.left)
                {
                    targetPosition = new Vector3(startPosition.x - moveDistance, startPosition.y, startPosition.z);
                    Flip();
                    Debug.Log("Flip()����");

                }
                else if (currentDirection == Vector3.right)
                {
                    targetPosition = new Vector3(startPosition.x + moveDistance, startPosition.y, startPosition.z);
                    Flip();
                    Debug.Log("Flip()����");
                }
                else if (currentDirection == Vector3.up)
                {
                    targetPosition = new Vector3(startPosition.x, startPosition.y + moveDistance, startPosition.z);
                    Debug.Log("����");
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
