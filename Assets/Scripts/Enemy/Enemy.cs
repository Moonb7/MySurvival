using System.Collections;
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
    private NavMeshAgent agent;
    private Rigidbody rb;

    private GameObject player;
    private EnemyState currentStats = EnemyState.Idle; // �̰Ŵ� ��� ���� �ൿ�̴�
    private float attackRange;
    private float countDown = 10f;
    [SerializeField]
    private float attackTime = 3f;
     
    [SerializeField]
    private int deathGold;         // ������ �÷��̾ ���Ե� ��差
    [SerializeField]
    private int deathExp;          // ������ �÷��̾ ���Ե� ����ġ��
    [SerializeField]
    private Item[] deathItem;      // ������ ����Ʈ���� ������ ����ǰ


    public Transform arrowPos;     // ȭ�� ������ġ �ӽúθ����� �Ұ��̴�. ó�� �����ɋ��� Enemy�� �ڽĿ�����Ʈ�� ����� �ִϸ��̼��� ȭ���� ��� �������� �ڽĿ�����Ʈ���� �ܺη� ���� ���ư��� ���鿹���̴�.
    public GameObject arrowPrefab; // ȭ�� ������Ʈ������

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
        attackRange = agent.stoppingDistance - 0.1f;            // agent�׺񿡼� ��ž�ϴ� ������ �����Ͽ� ������ �����־���.
        Invoke("StartChase", 1f);                               // ���� �� ���� �� 3���Ŀ� Player�� �����ϰ� ����
    }

    void StartChase() // ó�� ��� ��� �ϴ� �ٷ� Player���� ã�Ƽ� �����ϰ� ������� Start���� ���
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

                if (distance <= attackRange) // ���� ������ ������
                {
                    SetState(EnemyState.Attack);
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
    }
    private void ArrowShot() // �ִϸ��̼� �̺�Ʈ�Լ��� ������ Ư�� �������� ȭ�� �߻�
    {
        foreach (Transform childTransform in arrowPos.transform)
        {
            childTransform.SetParent(null); // �ڽ� ������Ʈ �и�
        }
    }
}
