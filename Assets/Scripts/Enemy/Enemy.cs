using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState //�ִϸ��̼��� ����������� �Ͽ���.
{
    Idle,   // ���
    Chase,  // Ÿ�� ã��
    Attack, // ����
}
public class Enemy : CharacterStats
{
    private NavMeshAgent agent;
    private GameObject player;
    private EnemyState currentStats = EnemyState.Idle; // �̰Ŵ� ��� ���� �ൿ�̴�
    private float attackRange;
    private float countDown;
    public float attackTime = 3f;

    [SerializeField]
    private int deathGold; // ������ �÷��̾ ���Ե� ��差
    [SerializeField]
    private Item deathItem; // ������ ����Ʈ���� ������ ����ǰ

    private bool IsAttack
    {
        get { return animator.GetBool(AnimString.Instance.isAttack); }
        set { animator.SetBool(AnimString.Instance.isAttack,value); }
    }
    private bool CanMove
    {
        get {return animator.GetBool(AnimString.Instance.canMove);}
    }

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackRange = agent.stoppingDistance - 0.1f;
        Invoke("StartChase", 1f); // ���� �� ���� �� 3���Ŀ� Player�� �����ϰ� ����
    }

    private void Update()
    {
        if (isDeath)
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
            }
            // �������̳� ����ǰ Ȯ���� ���� Ʈ���� �߰� ���� 
            // �׸��� ���� ������Ʈ�� �����Ͽ� Ʈ�������� �Ȱ��� �����Ͽ� ������ ���󰡴� �̺�Ʈ�� ���� ������ ���� �������� RigidBody�� Ȱ���Ͽ� ���󰡰� �ϸ�ɰ� ����.
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
}
