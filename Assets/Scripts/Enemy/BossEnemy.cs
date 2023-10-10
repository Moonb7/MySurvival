using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BossEnemy : Enemy
{
    public GameObject bossUI;
    public Rig rig;
    protected bool IsShouting // 공격하는지체크와 애니메이션파라미터를 같이 주었다.
    {
        get { return animator.GetBool(AnimString.Instance.isShouting); }
        set { animator.SetBool(AnimString.Instance.isShouting, value); }
    }

    protected bool IsFlying // 공격하는지체크와 애니메이션파라미터를 같이 주었다.
    {
        get { return animator.GetBool(AnimString.Instance.isFlying); }
        set { animator.SetBool(AnimString.Instance.isFlying, value); }
    }

    protected override void Start()
    {
        bossUI.SetActive(true); // 보스 UI 활성화
        base.Start();
        

        StartCoroutine(Think());
    }

    protected override void Update()
    {
        if (isDeath)
        {
            StopAllCoroutines();
            return;
        }
        if (enemyTypes == EnemyTypes.RangedEnemy) // 원거리적에게 플레이어가 가까이 오면 근접공격하게 변경
        {
            IsClose = Physics.CheckSphere(transform.position, checkRadius, playerMask, QueryTriggerInteraction.Ignore);
        }

        if (IsAttack) // 공격 중 일때는 잠시 멈추기
        {
            rig.weight = 1;
            if (IsShouting) // 샤우팅 공격은 안쳐다보게끔 만들자
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
        transform.LookAt(target);

        if(!IsClose) // 가까이 있지 않을때만 움직이게 하기
            agent.SetDestination(target);
    }

    IEnumerator Think() // 패턴을 입력할 함수 // 공격중이면 이 함수를 사용하면 안된다.
    {
        yield return new WaitForSeconds(attackTime);

        yield return new WaitUntil(() => IsAttack == false); // 공격중이 아니면 기다리기

        int randPattern = Random.Range(0, 5);
        switch(randPattern)
        {
            case 0:
            case 1:
                if(IsClose) // 근접 공격 범위에 있으면
                {
                    StartCoroutine(Pattern1()); // 확률 40%
                }
                else
                {
                    int rand = Random.Range(0, 2); // 나머지 패턴 실행
                    switch (rand)
                    {
                        case 0:
                            StartCoroutine(Pattern2());
                            break;
                        case 1:
                            StartCoroutine(Pattern3());
                            break;
                    }
                }
                break;
            case 2:
            case 3:
                StartCoroutine(Pattern2()); // 40%
                break;
            case 4:
                StartCoroutine(Pattern3()); // 20%
                break;
        }

    }

    IEnumerator Pattern1() // 근접 공격 패턴
    {
        IsAttack = true;
        yield return new WaitForSeconds(1f);
        StartCoroutine(Think());
    }

    IEnumerator Pattern2() // 날아서 불뿜는 패턴
    {
        IsFlying= true;
        yield return new WaitForSeconds(1.7f);
        IsAttack = true;
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Think());
    }

    IEnumerator Pattern3() // 바위를 소환 하여 유도 발사하는 패턴
    {
        yield return new WaitForSeconds(0.1f);
        IsAttack = true;
        IsShouting = true;
        StartCoroutine(Think());
    }

    void OutIsAttack() // 공격이 끝난 시점에 false시켜 줄거다 애니메이션 이벤트에 추가할 함수 
    {
        IsAttack = false;
        IsShouting = false;
    }

    void OutIsFlying()
    {
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

            DataManager.Instance.AddGold(deathGold); // 골드 획득
            PlayerStats.instance.AddExp(deathExp); // 경험치 획득
            EnemyManager.Instance.RemoveEnemy(this); // 죽음시 꼭 삭제해주기

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
}
