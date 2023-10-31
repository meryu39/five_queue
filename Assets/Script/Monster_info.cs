using UnityEngine;
using UnityEngine.UI;

public class Monster_info : MonoBehaviour
{ 
    float MonsterAttack = 5f;
    float moveSpeed = 2f;
    Transform playerTransform;
    bool isfollow = false;
    public int Monster_HP = 100;
    public GameObject M_area;
    public GameObject hpBarPrefab; // HP 바 프리팹
    private Slider Monster_hpbar; // 몬스터 체력바 
    private GameObject Monster_area;
    public GameObject UIparent;

    void Start()
    {
        if (hpBarPrefab != null)
        {
            Monster_hpbar = Instantiate(hpBarPrefab, transform.position, Quaternion.identity).GetComponent<Slider>();
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
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }

        if (Monster_hpbar != null)
        {
            Monster_hpbar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.05f, 0.3f, 0));
        }

        UpdateHP();
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
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 이 부분은 필요한 경우에만 사용합니다.
        }
    }
}
