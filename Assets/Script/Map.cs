using UnityEngine;

public class Map : MonoBehaviour
{
    private Renderer renderer;
    private Color originalColor;
    private bool playerInside;

    private void Start()
    {
        // ���� ������Ʈ�� Renderer ������Ʈ�� ��������
        renderer = GetComponent<Renderer>();

        // ���� ���� ����
        originalColor = renderer.material.color;

        // �ʱ⿡�� ���� �������� ����
        renderer.material.color = originalColor;

        // �÷��̾ �� �ȿ� ���� �ʱ� ����
        playerInside = false;
    }

    private void Update()
    {
        // �÷��̾ �� �ȿ� ���� ���� ������ ó��
        if (playerInside)
        {
            Color newColor = originalColor;
            newColor.a = 0.5f;
            renderer.material.color = newColor;
        }
        else
        {
            // �÷��̾ �� �ȿ� ���� ���� ���� �������� ����
            renderer.material.color = originalColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �÷��̾ �ʿ� ������ ��
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �÷��̾ ���� ������ ��
            playerInside = false;
        }
    }
}
