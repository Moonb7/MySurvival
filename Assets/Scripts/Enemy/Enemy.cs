using System.Collections;
using System.Xml.XPath;
using Unity.VisualScripting;
using UnityEditor;
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
    public EnemyTypes enemyTypes;
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public float beforSpeed;           // 일시정지후 다시 이동하기위해 스피드 저장값

    protected GameObject player;
    protected LayerMask playerMask;     // Player의 레이어지정
    public float closeCheckRadius = 0.5f;    // 플레이어가 어느정도 가까이 왔을때 감지할 범위
    public bool drawGizmo;              // 기즈모를 그릴지 말지 정하기

    public EnemyState currentStats = EnemyState.Idle; // 이거는 어떠한 상태 행동이다
    protected EnemyState beforStats;
    public float attackRange;
    protected float countDown = 10f;
    public float attackDelayTime = 3f;
    public float closeAttackTime = 3f;
    protected float distance;


    public int deathGold;               // 죽으면 플레이어가 갖게될 골드량
    public int deathExp;                // 죽으면 플레이어가 갖게될 경험치량
    private Item deathItem;             // 죽으면 플레이어가 갖게될 아이템

    public Transform arrowPos;          // 화살 생성위치 임시부모역할을 할것이다. 처음 생성될떄는 Enemy의 자식오브젝트로 만들고 애니메이션의 화살을 쏘는 시점에서 자식오브젝트말고 외부로 빠져 날아가게 만들예정이다.
    public GameObject arrowPrefab;      // 화살 오브젝트프리팹

    protected bool IsAttack // 공격하는지체크와 애니메이션파라미터를 같이 주었다.
    {
        get { return animator.GetBool(AnimString.Instance.isAttack); }
        set { animator.SetBool(AnimString.Instance.isAttack,value); }
    }

    protected bool IsClose // 원거리적에게만 근접공격이 가능하게 만들었다.
    {
        get {
            if (enemyTypes != EnemyTypes.RangedEnemy)
            {
                return false;
            }
                return animator.GetBool(AnimString.Instance.isClose); }
        set {
            if (enemyTypes != EnemyTypes.RangedEnemy)
            {
                return;
            }
            animator.SetBool(AnimString.Instance.isClose, value); }
    }

    protected bool CanMove //애니메이션파라미터를 같이 주었다.
    {
        get {return animator.GetBool(AnimString.Instance.canMove);}
    }

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerMask = LayerMask.GetMask("Player");
        SetState(EnemyState.Chase);
        EnemyManager.Instance.AddEnemy(this);
        beforSpeed = agent.speed;
    }

    protected virtual void Update()
    {
        if (isDeath)
            return;

        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        distance = Vector3.Distance(target, transform.position);

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
                  
                if (IsClose)
                {
                    SetState(EnemyState.IsClose);
                }
                break;

            case EnemyState.Attack:          // 공격모드로 변경
                if (countDown > attackDelayTime)
                {
                    StartCoroutine(LookTargetAttack(target));
                    countDown = 0;
                }
                if (distance > attackRange)  //다시 멀어지면
                {
                    SetState(EnemyState.Chase);
                }

                if (IsClose)
                {
                    SetState(EnemyState.IsClose);
                }
                break;

            case EnemyState.IsClose:
                if (countDown > closeAttackTime)
                {   
                    StartCoroutine(LookTargetAttack(target)); // 한번 쳐다 근접보고 공격
                    countDown = 0;
                }
                    
                if (IsClose == false)
                {
                    SetState(beforStats);
                }
                break;
        }

        countDown += Time.deltaTime;

        if (enemyTypes == EnemyTypes.RangedEnemy) // 원거리적에게 플레이어가 가까이 오면 근접공격하게 변경
        {
            IsClose = Physics.CheckSphere(transform.position, closeCheckRadius, playerMask, QueryTriggerInteraction.Ignore);
        }
    }


    protected virtual IEnumerator LookTargetAttack(Vector3 target)
    {
        transform.LookAt(target); // 공격할때 한번 쳐다봐야하는데
        IsAttack = true;
        yield return new WaitForSeconds(0.98f);
        IsAttack = false;
    }

    public virtual void SetState(EnemyState newState)
    {
        if (currentStats == newState) return;

        beforStats = currentStats;
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

            DataManager.Instance.AddGold(deathGold); // 골드 획득
            PlayerStats.instance.AddExp(deathExp); // 경험치 획득
            EnemyManager.Instance.RemoveEnemy(this); // 죽음시 꼭 삭제해주기

            // 확률을 구현해서 만들자
            float random = Random.Range(0,10);
            if(random <= 3) // 아이탬 얻기 30% 확률
            {
                int itemRandNum  = Random.Range(0 , ItemDataManager.Instance.items.Count); // 가장 첫번째 아이템 마지막 아이템 
                deathItem = ItemDataManager.Instance.items[itemRandNum];

                Inventory.Instance.AddItem(deathItem);
            }
            audioSource.clip = deathSound;      // 죽는 소리 플레이
            audioSource.Play();

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
            Destroy(this.gameObject, DeathDelay);
        }
    }

    public void DieEnemy() // 보스 등장시 일반 몹들은 일부러 죽일 것이다.
    {
        isDeath = true;

        if (animator != null)
        {
            animator.SetTrigger(AnimString.Instance.isDie);
            Debug.Log("죽음");
        }
        Destroy(this.gameObject, DeathDelay);
    }

    protected virtual void OnDrawGizmos()
    {
        if (enemyTypes != EnemyTypes.RangedEnemy || !drawGizmo)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, closeCheckRadius);
    }

    private void CreativeArrow() // 애니메이션 이벤트함수에 포함해 특정 구간에서 화살 생성
    {
        GameObject instance = Instantiate(arrowPrefab);
        instance.transform.SetParent(arrowPos);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
        //instance.GetComponent<Collider>().enabled = false;
    }
    private void ArrowShot() // 애니메이션 이벤트함수에 포함해 특정 구간에서 화살 발사
    {
        foreach (Transform childTransform in arrowPos.transform)
        {
            //childTransform.GetComponent<Collider>().enabled = true;
            childTransform.SetParent(null); // 자식 오브젝트 분리
        }
    }
}
