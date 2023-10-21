using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public enum BossPattern
{
    Pattern1,
    Pattern2,
    Pattern3,
    MaxNullPattern // 아무것도 없는것 초기화를 위해 만들었다
}
public class BossEnemy : Enemy
{
    public GameObject bossUI;
    public Rig rig;

    public AudioClip shoutingAtkSound;
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
            IsClose = Physics.CheckSphere(transform.position, closeCheckRadius, playerMask, QueryTriggerInteraction.Ignore);
        }

        if (IsAttack) // 공격 중 일때는 움직임 멈추기
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

        if(distance <= attackRange) // 공격 범위에 들어오면 패턴공격실행
        {
            StartCoroutine(Think());
        }

        transform.LookAt(target);

        if(!IsClose) // 가까이 있지 않을때만 움직이게 하기
            agent.SetDestination(target);
    }

    IEnumerator Think() // 패턴을 입력할 함수 // 공격중이면 이 함수를 사용하면 안된다.
    {
        countDown += Time.deltaTime;
        if (IsAttack && countDown < attackDelayTime) // 공격중이고 공격대기시간이 지나지 않으면 실행하지 않기
        {
            yield break;
        }

        IsAttack = true;

        int randPattern = Random.Range(0, 5);
        switch(randPattern)
        {
            case 0:
            case 1:
                if(IsClose) // 근접 공격 범위에 있으면
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
                Pattern2(); // 나는 패턴만 확률 20%센트로 조절
                break;
            case 4:
               Pattern3();
                break;
        }

    }

    void Pattern1() // 근접 공격 패턴
    {
        //IsAttack = true;

        animator.SetInteger("EnemyState", (int)BossPattern.Pattern1);
        // 확실하게 하기위해 초기화
        IsFlying = false;
        IsShouting = false;
    }

    void Pattern2() // 날아서 불뿜는 패턴
    {
        animator.SetInteger("EnemyState", (int)BossPattern.Pattern2);
        IsFlying = true;
        Collider collider = GetComponent<Collider>();
        collider.enabled= false;
        
    }

    void Pattern3() // 샤우팅하며 바위를 소환 하여 유도 발사하는 패턴
    {
        animator.SetInteger("EnemyState", (int)BossPattern.Pattern3);
        IsShouting = true;
        audioSource.clip = shoutingAtkSound;
        audioSource.Play();

        IsFlying = false;
    }

    void OutIsAttack() // 공격이 끝난 시점에 false시켜 줄거다 애니메이션 이벤트에 추가할 함수 
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
