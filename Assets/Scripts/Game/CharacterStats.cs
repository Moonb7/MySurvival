using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stats maxHealth;
    public float CurrentHealth { get; private set; }
    public Stats maxMana;
    public float CurrentMana { get; private set; }
    public Stats attack;
    public Stats defence;
    public bool isDeath = false;
    [SerializeField]
    private float DeathDelay = 3f;
    private Animator animator;
    [Tooltip("무적")]
    public bool Invincible { get; set; }                             // 구르기 할때 발동
    public bool CanPickUP() => CurrentHealth < maxHealth.GetValue(); // 힐 아이템을 먹을수 있는지 체크

    private AudioSource audioSource;
    public AudioClip hitSound1;                                      // 맞을떄 소리 랜덤으로 2가지 설정
    public AudioClip hitSound2;                                      // 맞을때 소리
    public GameObject hitEff;                                        // 히트 이펙트 효과

    private void Start()
    {
        SetStats();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    void SetStats()
    {
        CurrentHealth = maxHealth.GetValue();
        CurrentMana = maxMana.GetValue();
    }

    public void Heal(float amount)
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

    public void TakeDamage(float damage)
    {
        // 무적이면 데미지 처리X
        if (Invincible)
            return;

        float beforeHealth = CurrentHealth;
        CurrentHealth = CurrentHealth - (damage - defence.GetValue());
        if(CurrentHealth >= beforeHealth) // 데미지가 없으면
            CurrentHealth = beforeHealth; // 그냥 무데미지 적용
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth.GetValue());
        float realDamageAcount = beforeHealth - CurrentHealth; // real Damage구하기 데미지 입었는지 확인
        if (realDamageAcount > 0) // 데미지 구현 어떤 움직이나 맞았을때의 효과
        {
            HitSoundEffect();
            if (animator != null)
            {
                animator.SetTrigger(AnimString.Instance.hit);
            }
        }
        OnDeath();
    }

    void HitSoundEffect() // 맞을때 효과
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
        Instantiate(hitEff, transform); // 위치는 다시 확인 하기 맞은 부위를 지정하여 찾아서 생성 하게 할수도 있다.
    }

    // 마나사용
    public void UseMana(float amount)
    {
        float beforeMana = CurrentMana;
        CurrentMana -= amount;
        CurrentMana = Mathf.Clamp(CurrentMana, 0, maxMana.GetValue());
    }

    // 죽음
    void OnDeath()
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
    
}
