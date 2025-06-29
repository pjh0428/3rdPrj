using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    const float basicmoveSpeed = 5.0f;

    //에너미 상태변수
    private EnemyState m_State;

    //플레이어 발견 범위
    public float findDistance = 8.0f;

    //플레이어 트랜스폼
    Transform player;

    //애니메이터 변수
    Animator anim;

    //에너미 공격력
    public int attackPower = 5;

    //에너미 체력
    public int hp = 5;

  


    //공격범위
    public float attackDistance = 3.0f;
    //이속
    public float moveSpeed = 5.0f;

    //누적 시간
    float currentTime = 0;
    //공격 딜레이 시간
    float attackDelay = 2.0f;

    //초기 위치 저장용 변수
    Vector3 originPos;
    Quaternion originRot;

    //이동 가능 범위
    public float moveDisatnce = 20f;



    //캐릭터 컨트롤러 컴포넌트
    CharacterController cc;

    //내비게이션 에이전트 변수 
    NavMeshAgent zombieAgent;

    void Start()
    {


        //최초상태는 대기
        m_State = EnemyState.Idle;

        //플레이어으 ㅣ트랜스폼 컴포넌트 받아오기
        player = GameObject.FindGameObjectWithTag("Player").transform;


        //캐릭터 콘트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();

        originPos = transform.position;
        originRot = transform.rotation;

        anim = GetComponentInChildren<Animator>();
        zombieAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }
    void Idle()
    {
        //플레이어와의 거리가 액션 시작 범위 이내면 Move 상태로 전환
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;


            //이동 애니메이션으로 전환
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, originPos) > moveDisatnce)
        {
            m_State = EnemyState.Return;

            anim.SetTrigger("MoveToIdle"); // 필요하면 복귀 애니메이션 트리거 추가
            return;
        }

        if (Vector3.Distance(transform.position, player.position) <= attackDistance)
        {
            m_State = EnemyState.Attack;
 
            currentTime = attackDelay; // 공격 딜레이 초기화
            zombieAgent.isStopped = true;
            anim.SetTrigger("MoveToAttackDelay"); // 대기 or 공격 준비 애니메이션
            return;
        }

        // 그 외에는 계속 플레이어 추적
        zombieAgent.isStopped = false;
        zombieAgent.stoppingDistance = attackDistance;
        zombieAgent.SetDestination(player.position);
    }


    void Attack()
    {


        //만일 플레이어가 공격 범위 이내에 있다면 플레이어 공격
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            //일정한 시간마다 플레이어 공격
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
                Debug.Log("공격");
                currentTime = 0;
                moveSpeed = 0;
            }

            zombieAgent.isStopped = true;
            zombieAgent.ResetPath();

            //공격 애니메이션 플레이
            anim.SetTrigger("StartAttack");
        }
        //그렇지 않다면, 현재 상태를 이동상태로 전환
        else
        {
            zombieAgent.isStopped = true;
            zombieAgent.ResetPath();
        }
    }

    public void AttackAction()
    {
        //공격 애니메이션 시작시 이속 0
        moveSpeed = 0;
        //player.GetComponent<PlayerMove>().DamageAction(attackPower);

        zombieAgent.isStopped = true;
        zombieAgent.ResetPath();
    }

    public void AttackEnd()
    {
        //공격 애니메이션 끝나면 원래 이동 속도로 되돌리기
        moveSpeed = basicmoveSpeed;
        //현재 상태를 대기 상태로 전환
        m_State = EnemyState.Idle;
 

        //대기 애니메이션으로 전환하는 트랜지션 호출
        anim.SetTrigger("AttackToIdle");

        zombieAgent.isStopped = false;
        zombieAgent.SetDestination(player.position);
    }

    void Return()
    {
        //초기위치 거리가 0.1이상, 초기 위치쪽으로 이동
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            // Vector3 dir = (originPos - transform.position).normalized;
            // cc.Move(dir * moveSpeed * Time.deltaTime);

            // //방향을 복귀 지점으로 전환
            // transform.forward = dir;

            zombieAgent.SetDestination(originPos);


            zombieAgent.stoppingDistance = 0;

        }

        //그렇지 않다면, 자신위치 초기위치 조정, 현재상태 대기
        else
        {
            zombieAgent.isStopped = true;
            zombieAgent.ResetPath();

            transform.position = originPos;
            transform.rotation = originRot;

            m_State = EnemyState.Idle;
          

            //대기 애니메이션으로 전환하는 트랜지션 호출
            anim.SetTrigger("MoveToIdle");
        }
    }

    public void HitEnemy(int hitPower)
    {
        //만일 이미 피격 상태이거나 상망 상태 또는 복귀 상태라면 아무런 처리하지 않고 함수 종료
        if (m_State == EnemyState.Die )
            return;
        

        //플레이어 공격력 만큼 에네미 체력 감소
        hp -= hitPower;
        Debug.Log("좀비체력: " + hp);

        //내비게이션 에이전트의 이동을 멈추고 경로를 초기화
        zombieAgent.isStopped = true;
        zombieAgent.ResetPath();

        //에너미의 체력이 0보다 크면 피격 상태로 전환한다.
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;


            //피격 애니메이션
            anim.SetTrigger("Damaged");
            Damaged();
        }

        else
        {


            m_State = EnemyState.Die;
            

            //죽음 애니메이션
            anim.SetTrigger("Die");
            Die();
        }
    }

    void Damaged()
    {
        StartCoroutine("DamageProcess");
    }

    void Die()
    {
        //진행중인 피격 코루틴 중지
        StopCoroutine("DamageProcess");
        Destroy(gameObject, 2.5f); // 2.5초 후 시체 사라짐
    }

    IEnumerator DamageProcess()
    {
        //피격 모션 시간만큼 기다림
        yield return new WaitForSeconds(0.5f);

        //현재 상태를 이동상태로 전환
        anim.SetTrigger("DamagedToMove");
        m_State = EnemyState.Move;
   
    }

  
}
