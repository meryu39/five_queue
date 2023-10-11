using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    //해당 상호작용 오브젝트의 카테고리
    public InteractionManager.Category category;
    private Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }
    //여기서 부터는 임시로 상호작용 오브젝트를 강조한 것
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.localScale = Vector3.one * 0.1f + transform.localScale;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = Vector3.one * -0.1f + transform.localScale;
    }
}
