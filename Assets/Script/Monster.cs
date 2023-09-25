using UnityEngine;

public class Monster : MonoBehaviour
{
    float MonsterAttack = 5f;
    float moveSpeed = 5f;
    Transform playerTransform;
    bool isfollow = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            isfollow = true;
        }
    }

    private void Update()
    {
        if (isfollow && playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
          //몬스터 공격 관련 코드 
        }
    }
}
