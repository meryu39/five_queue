using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public enum ProjectileName
{
    NULL, SCALPEL, PIPE, BLOODPACK, FIREEXTINGUISHER
}

public class ProjectileCtrl : MonoBehaviour
{
    const float SCALPEL_SPEED = 10;
    const float SCALPEL_RANGE = 10;
    const bool SCALPEL_PENETRATION = true;
    const float SCALPEL_DAMAGE = 10;
    const float PIPE_SPEED = 10;
    const float PIPE_RANGE = 10;
    const bool PIPE_PENETRATION = true;
    const float PIPE_DAMAGE = 10;
    const float BLOODPACK_SPEED = 10;
    const float BLOODPACK_RANGE = 10;
    const bool BLOODPACK_PENETRATION = true;
    const float BLOODPACK_DAMAGE = 10;
    const float FIREEXTINGUISHER_SPEED = 10;
    const float FIREEXTINGUISHER_RANGE = 10;
    const bool FIREEXTINGUISHER_PENETRATION = true;
    const float FIREEXTINGUISHER_DAMAGE = 10;
    //투사체 상태정보
    public ProjectileName projectileName;
    public Vector2 throwingDirection;
    public float throwingSpeed;
    public float throwingRange;
    public float projectileDamage;
    public bool isPenetration;
    public float damage;
    //연산에 필요한 정보
    Transform transform;
    Vector2 throwingStartPos;
    Vector2 nowPos;

    void Start()
    {
        transform = GetComponent<Transform>();
        throwingStartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckRange();
        GetComponent<Rigidbody2D>().velocity = throwingDirection * throwingSpeed;
        //날라가는거
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Monster"))
        {
            Debug.Log("투사체" + projectileName + "적중");
            switch(projectileName)
            {
                case ProjectileName.SCALPEL:
                  
                    break;
            }
            if(!isPenetration)
            {
                DestroyProjectile();
            }
        }
    }

    public void Init(InteractionObjectName name, Vector2 dir)
    {
        throwingDirection = dir;
        switch (name)
        {
            case InteractionObjectName.SCALPEL:
                projectileName = ProjectileName.SCALPEL;
                throwingSpeed = SCALPEL_SPEED;
                throwingRange = SCALPEL_RANGE;
                projectileDamage = SCALPEL_DAMAGE;
                isPenetration = SCALPEL_PENETRATION;
                break;
            case InteractionObjectName.PIPE:
                projectileName = ProjectileName.PIPE;
                throwingSpeed = PIPE_SPEED;
                throwingRange = PIPE_RANGE;
                projectileDamage = PIPE_DAMAGE;
                isPenetration = PIPE_PENETRATION;
                break;
            case InteractionObjectName.BLOODPACK:
                projectileName = ProjectileName.BLOODPACK;
                throwingSpeed = BLOODPACK_SPEED;
                throwingRange = BLOODPACK_RANGE;
                projectileDamage = BLOODPACK_DAMAGE;
                isPenetration = BLOODPACK_PENETRATION;
                break;
            case InteractionObjectName.FIREEXTINGUISHER:
                projectileName = ProjectileName.FIREEXTINGUISHER;
                throwingSpeed = FIREEXTINGUISHER_SPEED;
                throwingRange = FIREEXTINGUISHER_RANGE;
                projectileDamage = FIREEXTINGUISHER_DAMAGE;
                isPenetration = FIREEXTINGUISHER_PENETRATION;
                break;
        }
    }
    private void DestroyProjectile()
    {
        switch(projectileName)
        {
            case ProjectileName.SCALPEL:
                break;
        }
        Destroy(gameObject);
    }
    private void CheckRange()
    {
        nowPos = transform.position;
        if((nowPos - throwingStartPos).magnitude > throwingRange)
        {
            DestroyProjectile();
        }
    }

}
