using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stats maxHealth;
    public float CurrentHealth { get; protected set; }
    public Stats attack;
    public Stats defence;
    public bool isDeath { get; protected set; }
    [SerializeField]
    protected float DeathDelay = 3f;
    protected Animator animator;
    [Tooltip("무적")]
    public bool Invincible { get; set; }                             // 무적으로 데미지가 않입게 했다. 구르기 할때 발동
    public bool CanPickUP() => CurrentHealth < maxHealth.GetValue(); // 힐 아이템을 먹을수 있는지 체크

    protected AudioSource audioSource;
    public AudioClip hitSound1;                                      // 맞을떄 소리 랜덤으로 2가지 설정
    public AudioClip hitSound2;                                      // 맞을때 소리
    public AudioClip deathSound;                                     // 죽는 소리
    public GameObject hitEff;                                        // 히트 이펙트 효과
    public Transform[] hitPos = new Transform[2];                    // 히트 위치 임의로 지정

    protected virtual void Start()
    {
        CurrentHealth = maxHealth.GetValue();

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void Heal(float amount)
    {
        if (CanPickUP() == false)
            return;

        float beforeHealth = CurrentHealth;
        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth.GetValue()); //현재 HP가 0밑으로는 않가고 최대체력을 넘지않게 만들어준다.

        // 힐 구하기 힐이 되었는지 확인
        float realHealAmount = CurrentHealth - beforeHealth;

        if(realHealAmount > 0)
        {
            // 힐효과 구현 게임창에 UI텍스쳐로 얼마나 회복했는지 보여주는걸 구현할수도 있을거 같다
        }
    }

    public virtual void TakeDamage(float damage)
    {
        // 무적이면 데미지 처리X
        if (Invincible)
            return;

        if (isDeath)
            return;

        float beforeHealth = CurrentHealth;
        CurrentHealth = CurrentHealth - (damage - defence.GetValue());
        if (CurrentHealth >= beforeHealth) // 데미지가 없으면
            //CurrentHealth = beforeHealth; // 그냥 무데미지 적용
            CurrentHealth -= 1; // 데미지 1이라도 입히게 하기
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth.GetValue());
        float realDamageAcount = beforeHealth - CurrentHealth; // real Damage구하기 데미지 입었는지 확인
        if (realDamageAcount > 0) // 데미지 구현 어떤 움직이나 맞았을때의 효과
        {
            HitEffect();
            if (animator != null)
            {
                animator.SetTrigger(AnimString.Instance.hit);
            }
        }

        OnDeath();
    }

    public virtual void HitEffect() // 맞을때 효과
    {
        int randomValue = Random.Range(0,2); // 맞을떄 두가지 패턴으로 변칙주기
        if(randomValue == 0)
        {
            audioSource.clip = hitSound1;
        }
        else if(randomValue == 1)
        {
            audioSource.clip = hitSound2;
        }
        audioSource.Play();

        int randm = Random.Range(0,2);
        if(hitPos.Length > 0)
        {
            GameObject instanc = Instantiate(hitEff, hitPos[randm]); // 위치는 다시 확인 하기 맞은 부위를 지정하여 찾아서 생성 하게 할수도 있다.
            instanc.transform.SetParent(null);
            Destroy(instanc, 2f);
        }

        if (gameObject.tag == "Player")
        {
            PlayerController.animator.SetBool(AnimString.Instance.isAttack, false);     // 스킬 및 공격을 할때마마다 true시켜준걸 false로 변환해주어 공격을 끝낸다.
            PlayerController.animator.SetInteger(AnimString.Instance.attackStats, -1);  // 플레이어가 공격을 당하면 공격 중인것을 초기화 시켜주었다.
        }
    }

    // 죽음
    public virtual void OnDeath()
    {
        if (isDeath) 
            return;

        if(CurrentHealth <= 0)
        {
            isDeath = true;
            // 죽음 구현 애니매이션이라던지 쉐이더를 활용하여
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            if(animator != null)
            {
                animator.SetTrigger(AnimString.Instance.isDie);
                Debug.Log("죽음");
            }

            if (gameObject.tag == "Player")
                return;

            Destroy(this.gameObject, DeathDelay);
        }
    }

    public void UseBuffItem(Item item)
    {
        ItemName itemName = (ItemName)item.number;
        switch (itemName)
        {
            case ItemName.AttackBuffPotion:
                StartCoroutine(AttBufPotion(item));
                break;
            case ItemName.DefenceBuffPotion:
                StartCoroutine(DefBufPotion(item));
                break;
        }
    }

    IEnumerator AttBufPotion(Item item) // 버프아이템 
    {
        attack.AddValue(item.value);
        yield return new WaitForSecondsRealtime(item.buffTime);
        attack.RemoveValue(item.value);
    }

    IEnumerator DefBufPotion(Item item) // 힐 아이템
    {
        defence.AddValue(item.value);
        yield return new WaitForSecondsRealtime(item.buffTime);
        defence.RemoveValue(item.value);
    }
}
