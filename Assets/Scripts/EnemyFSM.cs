using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public float findDistance = 8f; //범위
    public float attackDistance = 2f;
    public float moveDistance = 20f; //이동 가능 범위
    Vector3 originPos; //초기 위치
    Quaternion originRot;
    public float moveSpeed = 5f; //적 속도
    public int attackPower = 3; //적 공격력
    public int hp = 15;
    public int maxHp = 15; //체력,ㅊ ㅚ대체력
    public Slider hpSlider;

    float currentTime = 0;
    float attackDelay = 2f;

    Transform player;
    CharacterController cc;

    Animator anim;

    NavMeshAgent smith;

    //Vector3.Distance(A, B) : A와 B 사이의 거리를 반환하는 함수

    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    EnemyState m_State;

    void Start()
    {
        m_State=EnemyState.Idle; //대기상태
        player = GameObject.Find("Player").transform; 
        cc = GetComponent<CharacterController>();
        originPos = transform.position; //초기 위치 설정
        originRot = transform.rotation;

        anim = transform.GetComponentInChildren<Animator>(); //에너미의 자식(좀비모델)의 컴포넌트 받아오기

        smith = GetComponent<NavMeshAgent>();
    }

    void Idle() //대기 상태
    {
        if(Vector3.Distance(transform.position, player.position) < findDistance) //거리가 일정 거리 안이라면
        {
            m_State = EnemyState.Move;
            print("상태 전환: Idle -> Move"); //플레이어가 범위 안에 있으면 대기->이동 상태 전환

            anim.SetTrigger("IdleToMove"); //애니메이션 트리거주기
        }
    }
    void Move() //이동 상태 : 플레이어를 향해 이동
    {
        if(Vector3.Distance(transform.position, originPos) > moveDistance) //거리가 이동가능 범위 밖이라면
        {
            m_State = EnemyState.Return;
            print("상태 전환: Move -> Retrun"); //return호출, 이동->복귀 상태 전환
        }
        else if (Vector3.Distance(transform.position, player.position) > attackDistance) //거리가 공격 범위 밖이라면?
        {
            //Vector3 dir = (player.position - transform.position).normalized; //방향 설정(플레이어를 향해서)
            //cc.Move(dir * moveSpeed * Time.deltaTime); //cc컴포넌트.move를 이용해 이동
            //transform.forward = dir; //플레이어를 향해 방향 전환
            smith.isStopped = true; 
            smith.ResetPath(); //이동 중단, 경로 초기화

            smith.stoppingDistance = attackDistance; //목적지로 가는 최소거리를 공격가능 거리로
            smith.destination = player.position; //플레이어의 위치를 목적지로 설정
        }
        else //공격 범위 내에 있다면 이동->공격 상태 전환
        {
            m_State = EnemyState.Attack;
            print("상태 전환: Move -> Attack");

            currentTime = attackDelay;

            anim.SetTrigger("MoveToAttackD");
        }
    }
    void Attack() //공격 상태 : 플레이어에게 공격
    {
        if(Vector3.Distance(transform.position, player.position) < attackDistance) //플레이어가 공격 범위 안에 있다면 공격
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower); //Object.GetComponent.Method 해서 오브젝트의 컴포넌트의 메소드를 가져온다..ㅇㅎ
                print("공격");
                currentTime = 0;

                anim.SetTrigger("StartAttack");
            }
        }
        else //공격 범위 밖이라면 공격->이동 상태 전환
        {
            m_State = EnemyState.Move;
            print("상태 전환: Attack -> Move");
            currentTime = 0;

            anim.SetTrigger("AttackToMove");
        }
    }

    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    void Return() //초기 위치로 돌아가는 함수...
    {
        if (Vector3.Distance(transform.position, originPos) > 0.2f) //초기 위치와의 거리가 멀면 초기 위치 쪽으로 이동
        {
            //Vector3 dir = (originPos - transform.position).normalized;
            //cc.Move(dir * moveSpeed * Time.deltaTime);
            //transform.forward = dir;

            smith.destination = originPos;
            smith.stoppingDistance = 0;
        }
        else //초기 위치에 가까워지면 초기 위치로 이동, 복귀->대기 상태 전환
        {
            smith.isStopped = true;
            smith.ResetPath();

            transform.position = originPos;
            transform.rotation = originRot;
            hp = maxHp;
            m_State = EnemyState.Idle;
            print("상태 전환: Return -> Idle");

            anim.SetTrigger("MoveToIdle");
        }
    }
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(1.0f);
        m_State=EnemyState.Move;
        print("상태 전환: Damage -> Move");
    }
    IEnumerator DieProcess()
    {
        cc.enabled = false;
        yield return new WaitForSeconds(1.5f);
        print("소멸");
        Destroy(gameObject);
    }
    public void HitEnemy(int hitPower)
    {
        if(m_State == EnemyState.Damaged || m_State==EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }
        hp -= hitPower;

        smith.isStopped = true;
        smith.ResetPath();

        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any -> Damaged");

            anim.SetTrigger("Damaged");
            Damaged();
        }
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any -> Die");

            anim.SetTrigger("Die");
            Die();
        }
    }
    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }
    void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcess());
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
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;

        }

        hpSlider.value = (float)hp / (float)maxHp;
    }
}
