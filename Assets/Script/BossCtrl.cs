using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    static public BossCtrl instance;
    public GameObject player;
    public GameObject gameoverBackground;
    public GameObject shockObject;
    public GameObject runnerMob;
    public GameObject fallArea;
    public GameObject[] fallObject;
    public float shockCognitionRange = 30f;
    public float moveSpeed = 10f;
    public float shockSizeUpPerSecond = 1.0f;
    public int maxHp = 750;
    public bool isActive = false;
    public bool isCall = false;
    [SerializeField] private bool isPattern = false;
    [SerializeField] private float patternTime = 0;
    [SerializeField] private bool[] isPerformCrack = new bool[3] { false, false, false };
    [SerializeField] private float dustDecreasingSpeed = 1.0f;
    private Animator anim;
    private Monster_info state;
    public GameObject HPBarBackground;
    public GameObject HPBar;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject camera;

    private void Start()
    {
        instance = this;
        anim = GetComponent<Animator>();
        state = GetComponent<Monster_info>();
        HPBar.SetActive(false);
        HPBarBackground.SetActive(false);
    }

    private void Update()
    {
        HPBar.GetComponent<Image>().fillAmount = state.Monster_HP / maxHp;
        if (state.Monster_HP <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("EndingScene");
        }
        if (isActive == false)
        {
            isCall = false;
        }
        if (isActive && !isPattern)
        {
            ControllPattern();
        }
    }

    void ControllPattern()
    {
        patternTime += Time.deltaTime;
        if (patternTime > 75 && isPerformCrack[0] == false)
        {
            if (patternTime > 78)
            {
                isPerformCrack[0] = true;
                StartCoroutine(PerformCrack(1.5f, false));
            }
            else
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                if (direction.x > 0)
                {
                    anim.SetInteger("direction", 1);
                }
                else
                {
                    anim.SetInteger("direction", -1);
                }
                transform.Translate(direction * moveSpeed * dustDecreasingSpeed * Time.deltaTime);
            }
        }
        else if (patternTime > 154 && isPerformCrack[1] == false)
        {
            if (patternTime > 157)
            {
                isPerformCrack[1] = true;
                StartCoroutine(PerformCrack(1.5f, false));
            }
            else
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                if (direction.x > 0)
                {
                    anim.SetInteger("direction", 1);
                }
                else
                {
                    anim.SetInteger("direction", -1);
                }
                transform.Translate(direction * moveSpeed * dustDecreasingSpeed * Time.deltaTime);
            }
        }
        else if (patternTime > 171 && isPerformCrack[2] == false)
        {
            if (patternTime > 174)
            {
                isPerformCrack[2] = true;
                StartCoroutine(PerformCrack(6f, true));
            }
            else
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                if (direction.x > 0)
                {
                    anim.SetInteger("direction", 1);
                }
                else
                {
                    anim.SetInteger("direction", -1);
                }
                transform.Translate(direction * moveSpeed * dustDecreasingSpeed * Time.deltaTime);
            }
        }
        else if (Mathf.Abs(Vector3.Magnitude(player.transform.position - transform.position)) < shockCognitionRange)
        {
            StartCoroutine(PerformShock());
        }
        else if (isCall)
        {
            StartCoroutine(PerformCall());
        }
        else
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            if (direction.x > 0)
            {
                anim.SetInteger("direction", 1);
            }
            else
            {
                anim.SetInteger("direction", -1);
            }
            transform.Translate(direction * moveSpeed * dustDecreasingSpeed * Time.deltaTime);
        }
    }

    public void InitBoss()
    {
        isActive = true;
        state.Monster_HP = maxHp;
        HPBar.SetActive(true);
        HPBarBackground.SetActive(true);
    }

    IEnumerator PerformCrack(float duration, bool isEnd)
    {
        isPattern = true;
        player.GetComponent<PlayerMove>().canMove = false;
        anim.SetTrigger("crack");
        patternTime += duration;
        while (duration >= 0)
        {
            if (duration > 4.4f && duration < 4.6f)
            {
                anim.SetTrigger("crack");
            }
            if (duration > 2.9f && duration < 3.1f)
            {
                anim.SetTrigger("crack");
            }
            if (duration > 1.4f && duration < 1.6f)
            {
                anim.SetTrigger("crack");
            }
            //������� �־��� ������ ���� 1~4�� ������
            StartCoroutine(ShakeCamera());
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                Instantiate(fallObject[Random.Range(0, 3)], new Vector3(fallArea.transform.position.x + Random.Range(-fallArea.transform.localScale.x / 2, fallArea.transform.localScale.x / 2),
                                        fallArea.transform.position.y + Random.Range(-fallArea.transform.localScale.y / 2, fallArea.transform.localScale.y / 2),
                                        fallArea.transform.position.z), Quaternion.Euler(0, 0, 0));
            }

            if (isEnd == true)
            {
                Image gameoverBackgroundImage = gameoverBackground.GetComponent<Image>();
                gameoverBackgroundImage.color = new Color(gameoverBackgroundImage.color.r, gameoverBackgroundImage.color.g, gameoverBackgroundImage.color.b, gameoverBackgroundImage.color.a + 0.08f);
            }
            duration -= 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
        player.GetComponent<PlayerMove>().canMove = true;
        isPattern = false;
    }

    IEnumerator PerformShock()
    {
        isPattern = true;
        anim.SetTrigger("shock");
        yield return new WaitForSeconds(0.5f);
        GameObject initObject = Instantiate(shockObject, transform.position, Quaternion.Euler(0, 0, 0));
        for (int i = 0; i < 10; i++)
        {
            initObject.transform.localScale = new Vector3(initObject.transform.localScale.x + shockSizeUpPerSecond, initObject.transform.localScale.y + shockSizeUpPerSecond, initObject.transform.localScale.z);
            yield return new WaitForSeconds(0.05f);
        }
        patternTime += 1f;
        Destroy(initObject);
        isPattern = false;
    }

    IEnumerator PerformCall()
    {
        isPattern = true;
        anim.SetTrigger("call");
        yield return new WaitForSeconds(1f);
        Instantiate(runnerMob, new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
        Instantiate(runnerMob, new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(1f);
        patternTime += 2f;
        isCall = false;
        isPattern = false;
    }

    IEnumerator ShakeCamera()
    {
        virtualCamera.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            camera.transform.position = new Vector3(transform.position.x + Random.Range(-3, 4), transform.position.y + Random.Range(-3, 4), transform.position.z);
            yield return new WaitForSeconds(0.1f);
        }

        virtualCamera.gameObject.SetActive(true);
    }
}
