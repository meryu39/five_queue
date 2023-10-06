using UnityEngine;
using UnityEngine.UI;

public class Monster_info : MonoBehaviour
{
    float MonsterAttack = 5f;

    float moveSpeed = 2f;
    Transform playerTransform;

    bool isfollow = false;

    public int Monster_HP = 100; //몬스터 체력 100으로 고정

    public Slider Monster_hpbar; // 몬스터 체력바 
    public GameObject Del_hpbar; // 체력0이하시 제거될 몬스터 체력바
    void Start()
    {
        //몬스터 인식범위에 들어가지 않았을 경우 체력바 비활성화
        if (Monster_hpbar != null)
        {
            Monster_hpbar.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isfollow && playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized; //플레이어위치, 몬스터 위치 계산
            transform.Translate(direction * moveSpeed * Time.deltaTime); //플레이어 방향으로 이동 
        }
        if (Monster_hpbar != null)
        {
            Monster_hpbar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.05f, 0.3f, 0)); //몬스터 위의 월드좌표로 체력바 띄움
        }

        UpdateHP(); //몬스터 체력바 최신화
       

    }

    private void UpdateHP()
    {
        if (Monster_hpbar != null)
        { //몬스터 체력슬라이더 밸류값을 현재 몬스터 hp로 대입
            Monster_hpbar.value = (float)Monster_HP / 100;
        } 
        if (Monster_HP <= 0)  //몬스터 체력이 0이하일 경우
        { //몬스터 ui, 몬스터 삭제
            Destroy(Del_hpbar);   
            Destroy(gameObject);
            

        }
    }

    //몬스터 인식범위(콜라이더안에 플레이어의 콜라이더가 닿을 경우)
    private void OnTriggerEnter2D(Collider2D other)
    { //충돌시킨 태그가 플레이어 태그 일경우
        if (other.CompareTag("Player"))
        {
            //플레이어의 위치에 충돌된 오브젝트 위치값으로 설정
            playerTransform = other.transform;
            isfollow = true; //쫓아가기 활성화

            // GameObject로 활성화
            if (Monster_hpbar != null)
            {
                //인식범위에 식별된 경우 체력바 활성화
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
