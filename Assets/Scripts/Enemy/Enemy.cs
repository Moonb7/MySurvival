using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState //�ִϸ��̼��� ����������� �Ͽ���.
{
    Idle,   // ���
    Chase,  // Ÿ�� ã��
    Attack, // ����
    IsClose // ���� ������������ ���Ÿ������� �ʿ��Ѱ� ���� �־���.
}
public enum EnemyTypes
{
    MeleeEnemy,
    RangedEnemy
}

public class Enemy : CharacterStats
{
    public EnemyTypes enemyTypes;
    
    private NavMeshAgent agent;

    private GameObject player;
    private LayerMask playerMask;       // Player�� ���̾�����
    public float checkRadius = 0.5f;    // Physics.CheckSphere�� ������ ����
    public bool drawGizmo;              // ����� �׸��� ���� ���ϱ�

    private EnemyState currentStats = EnemyState.Idle; // �̰Ŵ� ��� ���� �ൿ�̴�
    private EnemyState beforStats;
    private float attackRange;
    private float countDown = 10f;
    public float attackTime = 3f;
    
    public int deathGold;               // ������ �÷��̾ ���Ե� ��差
    public int deathExp;                // ������ �÷��̾ ���Ե� ����ġ��
    public Item[] deathItem;            // ������ ����Ʈ���� ������ ����ǰ

    public Transform arrowPos;          // ȭ�� ������ġ �ӽúθ����� �Ұ��̴�. ó�� �����ɋ��� Enemy�� �ڽĿ�����Ʈ�� ����� �ִϸ��̼��� ȭ���� ��� �������� �ڽĿ�����Ʈ���� �ܺη� ���� ���ư��� ���鿹���̴�.
    public GameObject arrowPrefab;      // ȭ�� ������Ʈ������

    private bool IsAttack // �����ϴ���üũ�� �ִϸ��̼��Ķ���͸� ���� �־���.
    {
        get { return animator.GetBool(AnimString.Instance.isAttack); }
        set { animator.SetBool(AnimString.Instance.isAttack,value); }
    }

    private bool IsClose // ���Ÿ������Ը� ���������� �����ϰ� �������.
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

    private bool CanMove //�ִϸ��̼��Ķ���͸� ���� �־���.
    {
        get {return animator.GetBool(AnimString.Instance.canMove);}
    }

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerMask = LayerMask.GetMask("Player");
        attackRange = agent.stoppingDistance - 0.1f; // agent�׺񿡼� ��ž�ϴ� ������ �����Ͽ� ������ �����־���.
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

                if (distance <= attackRange) // ���� ������ ������
                {
                    SetState(EnemyState.Attack);
                }
                
                if (IsClose)
                {
                    SetState(EnemyState.IsClose);
                }
                break;

            case EnemyState.Attack:          // ���ݸ��� ����
                if (countDown > attackTime)
                {
                    StartCoroutine(Attack(target));
                    countDown = 0;
                }
                if (distance > attackRange)  //�ٽ� �־�����
                {
                    SetState(EnemyState.Chase);
                }

                if (IsClose)
                {
                    SetState(EnemyState.IsClose);
                }
                break;

            case EnemyState.IsClose:
                if(IsClose == false)
                {
                    SetState(beforStats);
                }
                break;
        }

        countDown += Time.deltaTime;

        if (enemyTypes == EnemyTypes.RangedEnemy) // ���Ÿ������� �÷��̾ ������ ���� ���������ϰ� ����
        {
            IsClose = Physics.CheckSphere(transform.position, checkRadius, playerMask, QueryTriggerInteraction.Ignore);
        }
    }

    private void OnDrawGizmos()
    {
        if (enemyTypes != EnemyTypes.RangedEnemy || !drawGizmo)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, checkRadius);
    }

    IEnumerator Attack(Vector3 target)
    {
        transform.LookAt(target); // �����Ҷ� �ѹ� �Ĵٺ����ϴµ�
        IsAttack = true;
        yield return new WaitForSeconds(0.98f);
        IsAttack = false;
    }

    void SetState(EnemyState newState)
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
            // ���� ���� �ִϸ��̼��̶���� ���̴��� Ȱ���Ͽ�

            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if(playerStats != null)
            {
                playerStats.AddGold(deathGold); // ��� ȹ��    
                playerStats.AddExp(deathExp);   // ����ġ ȹ��
            }

            audioSource.clip = deathSound;      // �״� �Ҹ� �÷���
            audioSource.Play();

            // �׸��� ���� ������Ʈ�� �����Ͽ� Ʈ�������� �Ȱ��� �����Ͽ� ������ ���󰡴� �̺�Ʈ�� ���� ������ ���� �������� RigidBody�� Ȱ���Ͽ� ���󰡰� �ϸ�ɰ� ����.
            // �̰Ŵ� �� ��� �� �̷��� ���� �ƴ� �����긦 �̿��ؼ� ����������� �����갡������ ���⵵�ϴ�

            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            if (animator != null)
            {
                animator.SetTrigger(AnimString.Instance.isDie);
                Debug.Log("����");
            }

            if (gameObject.tag != "Player")
            {
                Destroy(this.gameObject, DeathDelay);
            }
        }
    }
    
    private void CreativeArrow() // �ִϸ��̼� �̺�Ʈ�Լ��� ������ Ư�� �������� ȭ�� ����
    {
        GameObject instance = Instantiate(arrowPrefab);
        instance.transform.SetParent(arrowPos);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
        instance.GetComponent<Collider>().enabled = false;
    }
    private void ArrowShot() // �ִϸ��̼� �̺�Ʈ�Լ��� ������ Ư�� �������� ȭ�� �߻�
    {
        foreach (Transform childTransform in arrowPos.transform)
        {
            childTransform.GetComponent<Collider>().enabled = true;
            childTransform.SetParent(null); // �ڽ� ������Ʈ �и�
        }
    }
}
