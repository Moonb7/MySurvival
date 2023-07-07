using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState //�ִϸ��̼��� ����������� �Ͽ���.
{
    Idle,   // ���
    Chase,  // Ÿ�� ã��
    Attack, // ����
}
public class Enemy : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private CharacterStats stats; // �̰Ŵ� HP MP ��Ÿ ����̴�

    private GameObject player;

    private EnemyState currentStats = EnemyState.Idle; // �̰Ŵ� ��� ���� �ൿ�̴�

    private float attackRange;
    private float countDown;
    public float attackTime = 3f;

    private bool IsAttack
    {
        get { return animator.GetBool(AnimString.Instance.isAttack); }
        set { animator.SetBool(AnimString.Instance.isAttack,value); }
    }
    private bool CanMove
    {
        get { return animator.GetBool(AnimString.Instance.canMove); }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<CharacterStats>();
        player = GameObject.FindGameObjectWithTag("Player");

        attackRange = agent.stoppingDistance - 0.1f;

        Invoke("StartChase", 3f); // ���� �� ���� �� 3���Ŀ� Player�� �����ϰ� ����
    }

    private void Update()
    {
        if (stats.isDeath)
            return;

        Vector3 target = new Vector3(player.transform.position.x,transform.position.y,player.transform.position.z);
        float distance = Vector3.Distance(target, transform.position);

        switch (currentStats)
        {
            case EnemyState.Chase:
                if(animator.GetBool(AnimString.Instance.isGround) && CanMove)
                {
                    transform.LookAt(target);
                    agent.SetDestination(target);
                }

                if (distance <= attackRange) // ���� ������ ������
                {
                    SetState(EnemyState.Attack);
                }
                break;

            case EnemyState.Attack: // ���ݸ��� ����
                if (countDown > attackTime)
                {
                    StartCoroutine(Attack(target));
                    countDown = 0;
                }

                if (distance > attackRange) //�ٽ� �־�����
                {
                    SetState(EnemyState.Chase);
                }
                break;
        }

        countDown += Time.deltaTime;
    }


    IEnumerator Attack(Vector3 target)
    {
        transform.LookAt(target); // �����Ҷ� �ѹ� �Ĵٺ����ϴµ�
        IsAttack = true;
        yield return new WaitForSeconds(0.98f);
        IsAttack = false;
    }

    void StartChase() // ó�� ��� ��� �ϴ� �ٷ� Player���� ã�Ƽ� �����ϰ� ������� Start���� ���
    {
        SetState(EnemyState.Chase);
    }

    void SetState(EnemyState newState)
    {
        if (currentStats == newState) return;

        currentStats = newState;

        animator.SetInteger(AnimString.Instance.enemyState, (int)currentStats);
        
    }
}
