using UnityEngine;

//public State State;

public class Monster : MonoBehaviour
{
    float MonsterAttack = 5f;
    float moveSpeed = 2f;
    Transform playerTransform;
    bool isfollow = false;


    void Start()
    {
        //MyClassA State = GetComponent<MyClassA>();
        //int PlayerHP = otherClass.myVariable;
    }
    private void Update()
    {
        if (isfollow && playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            isfollow = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
          // State
        }
    }
}
