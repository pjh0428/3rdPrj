using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{

    #region 전투변수
    bool isDead; //좀비 사망여부
    public int hp = 5; //좀비 체력
    public int damage; //좀비 데미지
    public int attack=1; //좀비 공격력
    #endregion

    

    public float moveSpeed = 2.0f; //좀비 이동속도
    public float attackRange = 2.0f; //좀비 공격범위
    public float detectRange = 5.0f; //좀비 플레이어 감지범위

    public Transform player; //플레이어 위치

    Animator anim;
    NavMeshAgent agent;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; //태그로 플레이어 찾기
        agent.speed = moveSpeed; //NavMeshAgent 속도 설정

    }

    // Update is called once per frame
    void Update()
    {
        DectectPlayer();
    }

    public void DectectPlayer()
    {
        //플레이어 감지
        //플레이어 감지하면 공격
        //플레이어 감지하면 공격 애니메이션
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectRange && !isDead)
        {
            anim.SetBool("detect", true); //범위안에 있을 시 스크림
                                          //agent.SetDestination(player.position); //플레이어 위치로 이동
            if (distanceToPlayer <= attackRange)
            {
                anim.SetTrigger("attack");
                //공격 애니메이션
                AttackPlayer(); //플레이어 공격
            }
            else
            {
                //anim.SetBool("detect", false);
                agent.SetDestination(player.position); 
            }
        }
    }


    public void AttackPlayer()
    {
        //플레이어공격
        //플레이어에게 데미지

    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die(); //사망처리
        }
    }

    public void Die()
    {
        isDead = true;
        //사망 애니메이션 재생 후 3초에 사라짐
    }
}
