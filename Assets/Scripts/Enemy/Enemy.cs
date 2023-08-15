using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState //애니메이션은 변수순서대로 하였다.
{
    Idle,   // 대기
    Chase,  // 타켓 찾기
    Attack, // 공격
    IsClose // 적이 근접해있으면 원거리적에게 필요한거 같아 넣었다.
}
public enum EnemyTypes
{
    MeleeEnemy,
    RangedEnemy
}

public class Enemy : CharacterStats
{
    private NavMeshAgent agent;
    private Rigidbody rb;

    private GameObject player;
    private EnemyState currentStats = EnemyState.Idle; // 이거는 어떠한 상태 행동이다
    private float attackRange;
    private float countDown = 10f;
    [SerializeField]
    private float attackTime = 3f;
     
    [SerializeField]
    private int deathGold;         // 죽으면 플레이어가 갖게될 골드량
    [SerializeField]
    private int deathExp;          // 죽으면 플레이어가 갖게될 경험치량
    [SerializeField]
    private Item[] deathItem;      // 죽으면 떨어트리는 아이템 전리품


    public Transform arrowPos;     // 화살 생성위치 임시부모역할을 할것이다. 처음 생성될떄는 Enemy의 자식오브젝트로 만들고 애니메이션의 화살을 쏘는 시점에서 자식오브젝트말고 외부로 빠져 날아가게 만들예정이다.
    public GameObject arrowPrefab; // 화살 오브젝트프리팹

    private bool IsAttack
    {
        get { return animator.GetBool(AnimString.Instance.isAttack); }
        set { animator.SetBool(AnimString.Instance.isAttack,value); }
    }

    private bool CanMove
    {
        get {return animator.GetBool(AnimString.Instance.canMove);}
    }

    public EnemyTypes enemyTypes;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackRange = agent.stoppingDistance - 0.1f;            // agent네비에서 스탑하는 범위에 지정하여 범위를 맟춰주었다.
        Invoke("StartChase", 1f);                               // 시작 및 생성 후 3초후에 Player를 추적하게 만듬
    }

    void StartChase() // 처음 잠깐 대기 하다 바로 Player에게 찾아서 공격하게 만들었다 Start에서 사용
    {
        SetState(EnemyState.Chase);
    }

    private void Update()
    {
        if (isDeath)
            return;

        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
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

            case EnemyState.Attack:          // 공격모드로 변경
                if (countDown > attackTime)
                {
                    StartCoroutine(Attack(target));
                    countDown = 0;
                }
                if (distance > attackRange)  //다시 멀어지면
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

    void SetState(EnemyState newState)
    {
        if (currentStats == newState) return;

        currentStats = newState;

        animator.SetInteger(AnimString.Instance.enemyState, (int)currentStats);
    }

    public override void OnDeath()
    {
        if (isDeath)
            return;

        if (CurrentHealth <= 0)
        {
            isDeath = true;
            // 죽음 구현 애니매이션이라던지 쉐이더를 활용하여

            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if(playerStats != null)
            {
                playerStats.AddGold(deathGold); // 골드 획득    
                playerStats.AddExp(deathExp);   // 경험치 획득
            }

            audioSource.clip = deathSound;      // 죽는 소리 플레이
            audioSource.Play();

            // 그리고 랙돌 오브젝트를 생성하여 트랜스폼을 똑같이 생성하여 죽음시 날라가는 이벤트를 만들어도 좋을거 같다 몸통쪽의 RigidBody를 활용하여 날라가게 하면될거 같다.
            // 이거는 좀 고민 중 이렇게 할지 아님 디졸브를 이용해서 사라지게할지 디졸브가나은거 같기도하다

            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            if (animator != null)
            {
                animator.SetTrigger(AnimString.Instance.isDie);
                Debug.Log("죽음");
            }

            if (gameObject.tag != "Player")
            {
                Destroy(this.gameObject, DeathDelay);
            }
        }
    }
    
    private void CreativeArrow() // 애니메이션 이벤트함수에 포함해 특정 구간에서 화살 생성
    {
        GameObject instance = Instantiate(arrowPrefab);
        instance.transform.SetParent(arrowPos);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
    }
    private void ArrowShot() // 애니메이션 이벤트함수에 포함해 특정 구간에서 화살 발사
    {
        foreach (Transform childTransform in arrowPos.transform)
        {
            childTransform.SetParent(null); // 자식 오브젝트 분리
        }
    }
}
