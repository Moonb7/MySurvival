using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public enum BossPattern
{
    Pattern1,
    Pattern2,
    Pattern3,
}

public class BossEnemy : Enemy
{
    public GameObject bossUI;
    public Rig rig;
    private BossPattern bossPattern;

    protected bool IsFlying // �����ϴ���üũ�� �ִϸ��̼��Ķ���͸� ���� �־���.
    {
        get { return animator.GetBool(AnimString.Instance.isFlying); }
        set { animator.SetBool(AnimString.Instance.isFlying, value); }
    }

    protected override void Start()
    {
        bossPattern = BossPattern.Pattern1; // ������ 1�� ����
        bossUI.SetActive(true); // ���� UI Ȱ��ȭ
        base.Start();
    }

    protected override void Update()
    {
        if (isDeath)
            return;

        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        float distance = Vector3.Distance(target, transform.position);

        switch (bossPattern)
        {
            case BossPattern.Pattern1:
                switch (currentStats)
                {
                    case EnemyState.Idle:
                        break;

                    case EnemyState.Chase:
                        IsFlying = false;       // �����ʴ� �����Դϴ�.
                        rig.weight = 1;

                        if (CanMove)
                        {
                            transform.LookAt(target);
                            agent.SetDestination(target);
                        }
                        if (distance <= attackRange) // ���� ������ ������
                        {
                            SetState(EnemyState.Attack);
                        }
                        break;
                        
                    case EnemyState.Attack:
                        if (countDown > attackTime)
                        {
                            StartCoroutine(LookTargetAttack(target));
                            countDown = 0;
                        }
                        if (distance > attackRange)  //�ٽ� �־�����
                        {
                            SetState(EnemyState.Chase);
                        }
                        break;
                        
                    case EnemyState.IsClose:
                        break;
                }
                break;

            case BossPattern.Pattern2:
                switch (currentStats)
                {
                    case EnemyState.Idle:
                        break;

                    case EnemyState.Chase:
                        IsFlying = true;       // �����ʴ� �����Դϴ�.
                        rig.weight = 1;

                        if (CanMove)
                        {
                            transform.LookAt(target);
                            agent.SetDestination(target);
                        }
                        if (distance <= attackRange) // ���� ������ ������
                        {
                            SetState(EnemyState.Attack);
                        }
                        break;

                    case EnemyState.Attack:
                        if (countDown > attackTime)
                        {
                            StartCoroutine(LookTargetAttack(target));
                            countDown = 0;
                        }
                        if (distance > attackRange)  //�ٽ� �־�����
                        {
                            SetState(EnemyState.Chase);
                        }
                        break;

                    case EnemyState.IsClose:
                        break;
                }
                break;

            case BossPattern.Pattern3:
                switch (currentStats)
                {
                    case EnemyState.Idle:
                        break;

                    case EnemyState.Chase:
                        break;

                    case EnemyState.Attack:
                        break;

                    case EnemyState.IsClose:
                        break;
                }
                break;
        }
        countDown += Time.deltaTime;
    }

    public override void OnDeath()
    {
        if (isDeath)
            return;

        if (CurrentHealth <= 0)
        {
            isDeath = true;
            rig.weight= 0;
            // ���� ���� �ִϸ��̼��̶���� ���̴��� Ȱ���Ͽ�

            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.AddGold(deathGold); // ��� ȹ��    
                playerStats.AddExp(deathExp);   // ����ġ ȹ��
            }

            audioSource.clip = deathSound;      // �״� �Ҹ� �÷���
            audioSource.Play();

            // �׸��� ���� ������Ʈ�� �����Ͽ� Ʈ�������� �Ȱ��� �����Ͽ� ������ ���󰡴� �̺�Ʈ�� ���� ������ ���� �������� RigidBody�� Ȱ���Ͽ� ���󰡰� �ϸ�ɰ� ����.
            // �̰Ŵ� �� ���� �� �̷��� ���� �ƴ� �����긦 �̿��ؼ� ����������� �����갡������ ���⵵�ϴ�

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