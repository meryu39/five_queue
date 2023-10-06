using UnityEngine;

public class Monster_info : MonoBehaviour
{
    float MonsterAttack = 5f;

    float moveSpeed = 2f;
    Transform playerTransform;
    bool isfollow = false;

    public int Monster_HP = 100;

    public GameObject Monster_hpbar;

    void Start()
    {
        Monster_hpbar.SetActive(false);
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
            Monster_hpbar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0, 0));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            isfollow = true;
            Monster_hpbar.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

        }
    }
}
