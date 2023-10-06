using UnityEngine;
using UnityEngine.UI;

public class Monster_info : MonoBehaviour
{
    float MonsterAttack = 5f;

    float moveSpeed = 2f;
    Transform playerTransform;
    bool isfollow = false;

    public int Monster_HP = 100;

    public Slider Monster_hpbar; // Slider 타입으로 선언
    public GameObject Del_hpbar;
    void Start()
    {
        // 아래 코드에서 GameObject로 비활성화/활성화
        if (Monster_hpbar != null)
        {
            Monster_hpbar.gameObject.SetActive(false);
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
            Destroy(Del_hpbar);   
            Destroy(gameObject);
            

        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            isfollow = true;

            // GameObject로 활성화
            if (Monster_hpbar != null)
            {
                Monster_hpbar.gameObject.SetActive(true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

        }
    }
}
