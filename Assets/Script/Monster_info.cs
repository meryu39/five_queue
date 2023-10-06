using UnityEngine;
using UnityEngine.UI;

public class Monster_info : MonoBehaviour
{
    float MonsterAttack = 5f;

    float moveSpeed = 2f;
    Transform playerTransform;

    bool isfollow = false;

    public int Monster_HP = 100; //���� ü�� 100���� ����

    public Slider Monster_hpbar; // ���� ü�¹� 
    public GameObject Del_hpbar; // ü��0���Ͻ� ���ŵ� ���� ü�¹�
    void Start()
    {
        //���� �νĹ����� ���� �ʾ��� ��� ü�¹� ��Ȱ��ȭ
        if (Monster_hpbar != null)
        {
            Monster_hpbar.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isfollow && playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized; //�÷��̾���ġ, ���� ��ġ ���
            transform.Translate(direction * moveSpeed * Time.deltaTime); //�÷��̾� �������� �̵� 
        }
        if (Monster_hpbar != null)
        {
            Monster_hpbar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.05f, 0.3f, 0)); //���� ���� ������ǥ�� ü�¹� ���
        }

        UpdateHP(); //���� ü�¹� �ֽ�ȭ
       

    }

    private void UpdateHP()
    {
        if (Monster_hpbar != null)
        { //���� ü�½����̴� ������� ���� ���� hp�� ����
            Monster_hpbar.value = (float)Monster_HP / 100;
        } 
        if (Monster_HP <= 0)  //���� ü���� 0������ ���
        { //���� ui, ���� ����
            Destroy(Del_hpbar);   
            Destroy(gameObject);
            

        }
    }

    //���� �νĹ���(�ݶ��̴��ȿ� �÷��̾��� �ݶ��̴��� ���� ���)
    private void OnTriggerEnter2D(Collider2D other)
    { //�浹��Ų �±װ� �÷��̾� �±� �ϰ��
        if (other.CompareTag("Player"))
        {
            //�÷��̾��� ��ġ�� �浹�� ������Ʈ ��ġ������ ����
            playerTransform = other.transform;
            isfollow = true; //�Ѿư��� Ȱ��ȭ

            // GameObject�� Ȱ��ȭ
            if (Monster_hpbar != null)
            {
                //�νĹ����� �ĺ��� ��� ü�¹� Ȱ��ȭ
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
