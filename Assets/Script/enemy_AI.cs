using UnityEngine;
using System.Collections;

public class enemy_AI : MonoBehaviour
{
    public float speed = 1.0f; // �̵� �ӵ�
    public float moveDistance = 2.0f; // �̵� �Ÿ�
    public float changeDirectionInterval = 2.0f; // ���� ���� ����
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 currentDirection;
    private bool isMoving = true;

    void Start()
    {
        startPosition = transform.position;
        currentDirection = RandomDirection();
        targetPosition = startPosition + currentDirection * moveDistance;
        StartCoroutine(ChangeDirection());
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position += currentDirection * speed * Time.deltaTime;

            // ���� ��ġ�� ��ǥ ��ġ ������ �Ÿ��� ���
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            if (distanceToTarget < 0.1f)
            {
                // ��ǥ ��ġ�� �����ϸ� ������ �ݴ�� �����ϰ� ��ǥ ��ġ�� ���� ��ġ�� ����
                currentDirection = -currentDirection;

                if (currentDirection == Vector3.left)
                    targetPosition = new Vector3(startPosition.x - moveDistance, startPosition.y, startPosition.z);
                else if (currentDirection == Vector3.right)
                    targetPosition = new Vector3(startPosition.x + moveDistance, startPosition.y, startPosition.z);
                else if (currentDirection == Vector3.up)
                    targetPosition = new Vector3(startPosition.x, startPosition.y + moveDistance, startPosition.z);
                else if (currentDirection == Vector3.down)
                    targetPosition = new Vector3(startPosition.x, startPosition.y - moveDistance, startPosition.z);
            }
        }
    }


    IEnumerator ChangeDirection()
    {
        while (true)
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
