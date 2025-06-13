using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{

    #region 전투변수
    bool isDead; //좀비 사망여부
    public int hp = 5; //좀비 체력
    public int damage; //좀비 데미지
    public int attack = 1; //좀비 공격력
    #endregion



    public float moveSpeed = 2.0f; //좀비 이동속도
    public float attackRange = 1.0f; //좀비 공격범위
    public float detectRange = 3.0f; //좀비 플레이어 감지범위

    public Transform player; //플레이어 위치

    Animator anim;
    NavMeshAgent agent;
    Transform target;
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
        MoveToPlayer();
    }

    public void MoveToPlayer()
    {
        agent.SetDestination(player.position);
        anim.SetFloat("speed", 1f, 0.3f, Time.deltaTime);
        RotateToPlayer();

        float distanceToTarget = Vector3.Distance(player.position, transform.position);

        if (distanceToTarget <= agent.stoppingDistance)
        {
            anim.SetFloat("speed", 0f);
            //Attack
            AttackPlayer();
        }
    }
    public void RotateToPlayer()
    {
        transform.LookAt(player);
        //플레이어를 바라봄
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
    }

    public void AttackPlayer()
    {
        //플레이어공격
        //플레이어에게 데미지
        anim.SetTrigger("attack");
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

    public void WallCheck()
    {

    }

    public void groundCheck()
    {

    }

    
}
