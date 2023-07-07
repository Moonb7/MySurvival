using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState //애니메이션은 변수순서대로 하였다.
{
    Idle,   // 대기
    Chase,  // 타켓 찾기
    Attack, // 공격
}
public class Enemy : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private CharacterStats stats; // 이거는 HP MP 기타 등등이다

    private GameObject player;

    private EnemyState currentStats = EnemyState.Idle; // 이거는 어떠한 상태 행동이다

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

        Invoke("StartChase", 3f); // 시작 및 생성 후 3초후에 Player를 추적하게 만듬
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

                if (distance <= attackRange) // 공격 범위에 있으면
                {
                    SetState(EnemyState.Attack);
                }
                break;

            case EnemyState.Attack: // 공격모드로 변경
                if (countDown > attackTime)
                {
                    StartCoroutine(Attack(target));
                    countDown = 0;
                }

                if (distance > attackRange) //다시 멀어지면
                {
                    SetState(EnemyState.Chase);
                }
                break;
        }

        countDown += Time.deltaTime;
    }


    IEnumerator Attack(Vector3 target)
    {
        transform.LookAt(target); // 공격할때 한번 쳐다봐야하는데
        IsAttack = true;
        yield return new WaitForSeconds(0.98f);
        IsAttack = false;
    }

    void StartChase() // 처음 잠깐 대기 하다 바로 Player에게 찾아서 공격하게 만들었다 Start에서 사용
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
