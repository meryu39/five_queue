using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove: MonoBehaviour
{
    public float moveSpeed = 5.0f; // �̵� �ӵ� ���� ����

    private Rigidbody2D rb; // ĳ������ Rigidbody2D ������Ʈ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ ��������
    }

    void Update()
    {
        // ����� �Է� ó��
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // �̵� ���� ���
        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

        // ĳ���� �̵�
        rb.velocity = moveDirection * moveSpeed;
    }
}
