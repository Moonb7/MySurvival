using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public enum BossPattern
{
    Pattern1,
    Pattern2,
    Pattern3,
    MaxNullPattern // �ƹ��͵� ���°� �ʱ�ȭ�� ���� �������
}
public class BossEnemy : Enemy
{
    public GameObject bossUI;
    public Rig rig;

    public AudioClip shoutingAtkSound;
    protected bool IsShouting // �����ϴ���üũ�� �ִϸ��̼��Ķ���͸� ���� �־���.
    {
        get { return animator.GetBool(AnimString.Instance.isShouting); }
        set { animator.SetBool(AnimString.Instance.isShouting, value); }
    }

    protected bool IsFlying // �����ϴ���üũ�� �ִϸ��̼��Ķ���͸� ���� �־���.
    {
        get { return animator.GetBool(AnimString.Instance.isFlying); }
        set { animator.SetBool(AnimString.Instance.isFlying, value); }
    }

    protected override void Start()
    {
        bossUI.SetActive(true); // ���� UI Ȱ��ȭ
        base.Start();
    }

    protected override void Update()
    {
        if (isDeath)
        {
            StopAllCoroutines();
            return;
        }

        if (enemyTypes == EnemyTypes.RangedEnemy) // ���Ÿ������� �÷��̾ ������ ���� ���������ϰ� ����
        {
            IsClose = Physics.CheckSphere(transform.position, closeCheckRadius, playerMask, QueryTriggerInteraction.Ignore);
        }

        if (IsAttack) // ���� �� �϶��� ������ ���߱�
        {
            rig.weight = 1;
            if (IsShouting) // ������ ������ ���Ĵٺ��Բ� ������
            {
                rig.weight = 0.5f;
            }
            return;
        }
        else
        {
            rig.weight = 0.5f;
        }
            
        
        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        distance = Vector3.Distance(target, transform.position);

        if(distance <= attackRange) // ���� ������ ������ ���ϰ��ݽ���
        {
            StartCoroutine(Think());
        }

        transform.LookAt(target);

        if(!IsClose) // ������ ���� �������� �����̰� �ϱ�
            agent.SetDestination(target);
    }

    IEnumerator Think() // ������ �Է��� �Լ� // �������̸� �� �Լ��� ����ϸ� �ȵȴ�.
    {
        countDown += Time.deltaTime;
        if (IsAttack && countDown < attackDelayTime) // �������̰� ���ݴ��ð��� ������ ������ �������� �ʱ�
        {
            yield break;
        }

        IsAttack = true;

        int randPattern = Random.Range(0, 5);
        switch(randPattern)
        {
            case 0:
            case 1:
                if(IsClose) // ���� ���� ������ ������
                {
                    Pattern1();
                }
                else
                {
                    Pattern3();
                }
                break;
            case 2:
                Pattern3();
                break;
            case 3:
                Pattern2(); // ���� ���ϸ� Ȯ�� 20%��Ʈ�� ����
                break;
            case 4:
               Pattern3();
                break;
        }

    }

    void Pattern1() // ���� ���� ����
    {
        //IsAttack = true;

        animator.SetInteger("EnemyState", (int)BossPattern.Pattern1);
        // Ȯ���ϰ� �ϱ����� �ʱ�ȭ
        IsFlying = false;
        IsShouting = false;
    }

    void Pattern2() // ���Ƽ� �һմ� ����
    {
        animator.SetInteger("EnemyState", (int)BossPattern.Pattern2);
        IsFlying = true;
        Collider collider = GetComponent<Collider>();
        collider.enabled= false;
        
    }

    void Pattern3() // �������ϸ� ������ ��ȯ �Ͽ� ���� �߻��ϴ� ����
    {
        animator.SetInteger("EnemyState", (int)BossPattern.Pattern3);
        IsShouting = true;
        audioSource.clip = shoutingAtkSound;
        audioSource.Play();

        IsFlying = false;
    }

    void OutIsAttack() // ������ ���� ������ false���� �ٰŴ� �ִϸ��̼� �̺�Ʈ�� �߰��� �Լ� 
    {
        IsAttack = false; 
        IsShouting = false;

        animator.SetInteger("EnemyState", (int)BossPattern.MaxNullPattern);
    }

    void OutIsFlying()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = true;

        IsAttack = false;
        IsFlying = false;

        animator.SetInteger("EnemyState", (int)BossPattern.MaxNullPattern);
    }

    public override void OnDeath()
    {
        if (isDeath)
            return;

        if (CurrentHealth <= 0)
        {
            isDeath = true;
            rig.weight= 0;

            DataManager.Instance.AddGold(deathGold); // ��� ȹ��
            PlayerStats.instance.AddExp(deathExp); // ����ġ ȹ��
            EnemyManager.Instance.RemoveEnemy(this); // ������ �� �������ֱ�

            audioSource.clip = deathSound;      // �״� �Ҹ� �÷���
            audioSource.Play();

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
            Destroy(this.gameObject, DeathDelay);
        }
    }
}
