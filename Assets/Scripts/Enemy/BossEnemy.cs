using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BossEnemy : Enemy
{
    public GameObject bossUI;
    public Rig rig;
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
        if (IsAttack && countDown < attackDelayTime)
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
                    Pattern1(); // Ȯ�� 40%
                }
                else
                {
                    int rand = Random.Range(0, 2); // ������ ���� ����
                    switch (rand)
                    {
                        case 0:
                            Pattern2();
                            break;
                        case 1:
                            Pattern3();
                            break;
                    }
                }
                break;
            case 2:
            case 3:
                Pattern2(); // 40%
                break;
            case 4:
               Pattern3(); // 20%
                break;
        }

    }

    void Pattern1() // ���� ���� ����
    {
        
    }

    void Pattern2() // ���Ƽ� �һմ� ����
    {
        IsFlying= true;
        Collider collider = GetComponent<Collider>();
        collider.enabled= false;
        
    }

    void Pattern3() // ������ ��ȯ �Ͽ� ���� �߻��ϴ� ����
    {
        IsShouting = true;
    }

    void OutIsAttack() // ������ ���� ������ false���� �ٰŴ� �ִϸ��̼� �̺�Ʈ�� �߰��� �Լ� 
    {
        IsAttack = false; 
        IsShouting = false;
    }

    void OutIsFlying()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = true;

        IsAttack = false;
        IsFlying = false;
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
