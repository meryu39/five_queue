using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove: MonoBehaviour
{
    public float moveSpeed = 5.0f; // 이동 속도 조절 변수

    private Rigidbody2D rb; // 캐릭터의 Rigidbody2D 컴포넌트

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 가져오기
    }

    void Update()
    {
        // 사용자 입력 처리
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 이동 벡터 계산
        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

        // 캐릭터 이동
        rb.velocity = moveDirection * moveSpeed;
    }
}
