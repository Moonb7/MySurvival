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

    protected bool IsFlying // 공격하는지체크와 애니메이션파라미터를 같이 주었다.
    {
        get { return animator.GetBool(AnimString.Instance.isFlying); }
        set { animator.SetBool(AnimString.Instance.isFlying, value); }
    }

    protected override void Start()
    {
        bossPattern = BossPattern.Pattern1; // 시작은 1번 부터
        bossUI.SetActive(true); // 보스 UI 활성화
        base.Start();
    }

    protected override void Update()
    {
        if (isDeath)
            return;

        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        float distance = Vector3.Distance(target, transform.position);

        switch (currentStats)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Chase:
                IsFlying = false;       // 날지않는 패턴입니다.
                rig.weight = 1;
                if (CanMove)
                {
                    transform.LookAt(target);
                    agent.SetDestination(target);
                }
                if (distance <= attackRange) // 공격 범위에 있으면
                {
                    SetState(EnemyState.Attack);
                }
                break;
            case EnemyState.Attack:
                switch (bossPattern)
                {
                    case BossPattern.Pattern1:
                        if (countDown > attackTime)
                        {
                            StartCoroutine(LookTargetAttack(target));
                            countDown = 0;
                        }
                        if (distance > attackRange)  //다시 멀어지면
                        {
                            SetState(EnemyState.Chase);
                        }
                        break;
                    case BossPattern.Pattern2:

                        break;
                    case BossPattern.Pattern3:

                        break;
                }
                break;
            case EnemyState.IsClose:
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
            // 죽음 구현 애니매이션이라던지 쉐이더를 활용하여

            DataManager.Instance.AddGold(deathGold); // 골드 획득
            PlayerStats.instance.AddExp(deathExp); // 경험치 획득
            EnemyManager.Instance.RemoveEnemy(this); // 죽음시 꼭 삭제해주기

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
            Destroy(this.gameObject, DeathDelay);
        }
    }
}
