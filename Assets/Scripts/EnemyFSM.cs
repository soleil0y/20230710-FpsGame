using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public float findDistance = 8f; //����
    public float attackDistance = 2f;
    public float moveDistance = 20f; //�̵� ���� ����
    Vector3 originPos; //�ʱ� ��ġ
    Quaternion originRot;
    public float moveSpeed = 5f; //�� �ӵ�
    public int attackPower = 3; //�� ���ݷ�
    public int hp = 15;
    public int maxHp = 15; //ü��,�� �ʴ�ü��
    public Slider hpSlider;

    float currentTime = 0;
    float attackDelay = 2f;

    Transform player;
    CharacterController cc;

    Animator anim;

    NavMeshAgent smith;

    //Vector3.Distance(A, B) : A�� B ������ �Ÿ��� ��ȯ�ϴ� �Լ�

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
        m_State=EnemyState.Idle; //������
        player = GameObject.Find("Player").transform; 
        cc = GetComponent<CharacterController>();
        originPos = transform.position; //�ʱ� ��ġ ����
        originRot = transform.rotation;

        anim = transform.GetComponentInChildren<Animator>(); //���ʹ��� �ڽ�(�����)�� ������Ʈ �޾ƿ���

        smith = GetComponent<NavMeshAgent>();
    }

    void Idle() //��� ����
    {
        if(Vector3.Distance(transform.position, player.position) < findDistance) //�Ÿ��� ���� �Ÿ� ���̶��
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ: Idle -> Move"); //�÷��̾ ���� �ȿ� ������ ���->�̵� ���� ��ȯ

            anim.SetTrigger("IdleToMove"); //�ִϸ��̼� Ʈ�����ֱ�
        }
    }
    void Move() //�̵� ���� : �÷��̾ ���� �̵�
    {
        if(Vector3.Distance(transform.position, originPos) > moveDistance) //�Ÿ��� �̵����� ���� ���̶��
        {
            m_State = EnemyState.Return;
            print("���� ��ȯ: Move -> Retrun"); //returnȣ��, �̵�->���� ���� ��ȯ
        }
        else if (Vector3.Distance(transform.position, player.position) > attackDistance) //�Ÿ��� ���� ���� ���̶��?
        {
            //Vector3 dir = (player.position - transform.position).normalized; //���� ����(�÷��̾ ���ؼ�)
            //cc.Move(dir * moveSpeed * Time.deltaTime); //cc������Ʈ.move�� �̿��� �̵�
            //transform.forward = dir; //�÷��̾ ���� ���� ��ȯ
            smith.isStopped = true; 
            smith.ResetPath(); //�̵� �ߴ�, ��� �ʱ�ȭ

            smith.stoppingDistance = attackDistance; //�������� ���� �ּҰŸ��� ���ݰ��� �Ÿ���
            smith.destination = player.position; //�÷��̾��� ��ġ�� �������� ����
        }
        else //���� ���� ���� �ִٸ� �̵�->���� ���� ��ȯ
        {
            m_State = EnemyState.Attack;
            print("���� ��ȯ: Move -> Attack");

            currentTime = attackDelay;

            anim.SetTrigger("MoveToAttackD");
        }
    }
    void Attack() //���� ���� : �÷��̾�� ����
    {
        if(Vector3.Distance(transform.position, player.position) < attackDistance) //�÷��̾ ���� ���� �ȿ� �ִٸ� ����
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower); //Object.GetComponent.Method �ؼ� ������Ʈ�� ������Ʈ�� �޼ҵ带 �����´�..����
                print("����");
                currentTime = 0;

                anim.SetTrigger("StartAttack");
            }
        }
        else //���� ���� ���̶�� ����->�̵� ���� ��ȯ
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ: Attack -> Move");
            currentTime = 0;

            anim.SetTrigger("AttackToMove");
        }
    }

    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    void Return() //�ʱ� ��ġ�� ���ư��� �Լ�...
    {
        if (Vector3.Distance(transform.position, originPos) > 0.2f) //�ʱ� ��ġ���� �Ÿ��� �ָ� �ʱ� ��ġ ������ �̵�
        {
            //Vector3 dir = (originPos - transform.position).normalized;
            //cc.Move(dir * moveSpeed * Time.deltaTime);
            //transform.forward = dir;

            smith.destination = originPos;
            smith.stoppingDistance = 0;
        }
        else //�ʱ� ��ġ�� ��������� �ʱ� ��ġ�� �̵�, ����->��� ���� ��ȯ
        {
            smith.isStopped = true;
            smith.ResetPath();

            transform.position = originPos;
            transform.rotation = originRot;
            hp = maxHp;
            m_State = EnemyState.Idle;
            print("���� ��ȯ: Return -> Idle");

            anim.SetTrigger("MoveToIdle");
        }
    }
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(1.0f);
        m_State=EnemyState.Move;
        print("���� ��ȯ: Damage -> Move");
    }
    IEnumerator DieProcess()
    {
        cc.enabled = false;
        yield return new WaitForSeconds(1.5f);
        print("�Ҹ�");
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
            print("���� ��ȯ: Any -> Damaged");

            anim.SetTrigger("Damaged");
            Damaged();
        }
        else
        {
            m_State = EnemyState.Die;
            print("���� ��ȯ: Any -> Die");

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
