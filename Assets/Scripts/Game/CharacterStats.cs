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
    [Tooltip("����")]
    public bool Invincible { get; set; }                             // �������� �������� ���԰� �ߴ�. ������ �Ҷ� �ߵ�
    public bool CanPickUP() => CurrentHealth < maxHealth.GetValue(); // �� �������� ������ �ִ��� üũ

    protected AudioSource audioSource;
    public AudioClip hitSound1;                                      // ������ �Ҹ� �������� 2���� ����
    public AudioClip hitSound2;                                      // ������ �Ҹ�
    public AudioClip deathSound;                                     // �״� �Ҹ�
    public GameObject hitEff;                                        // ��Ʈ ����Ʈ ȿ��
    public Transform[] hitPos = new Transform[2];                    // ��Ʈ ��ġ ���Ƿ� ����

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
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth.GetValue()); //���� HP�� 0�����δ� �ʰ��� �ִ�ü���� �����ʰ� ������ش�.

        // �� ���ϱ� ���� �Ǿ����� Ȯ��
        float realHealAmount = CurrentHealth - beforeHealth;

        if(realHealAmount > 0)
        {
            // ��ȿ�� ���� ����â�� UI�ؽ��ķ� �󸶳� ȸ���ߴ��� �����ִ°� �����Ҽ��� ������ ����
        }
    }

    public virtual void TakeDamage(float damage)
    {
        // �����̸� ������ ó��X
        if (Invincible)
            return;

        if (isDeath)
            return;

        float beforeHealth = CurrentHealth;
        CurrentHealth = CurrentHealth - (damage - defence.GetValue());
        if (CurrentHealth >= beforeHealth) // �������� ������
            //CurrentHealth = beforeHealth; // �׳� �������� ����
            CurrentHealth -= 1; // ������ 1�̶� ������ �ϱ�
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth.GetValue());
        float realDamageAcount = beforeHealth - CurrentHealth; // real Damage���ϱ� ������ �Ծ����� Ȯ��
        if (realDamageAcount > 0) // ������ ���� � �����̳� �¾������� ȿ��
        {
            HitEffect();
            if (animator != null)
            {
                animator.SetTrigger(AnimString.Instance.hit);
            }
        }

        OnDeath();
    }

    public virtual void HitEffect() // ������ ȿ��
    {
        int randomValue = Random.Range(0,2); // ������ �ΰ��� �������� ��Ģ�ֱ�
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
            GameObject instanc = Instantiate(hitEff, hitPos[randm]); // ��ġ�� �ٽ� Ȯ�� �ϱ� ���� ������ �����Ͽ� ã�Ƽ� ���� �ϰ� �Ҽ��� �ִ�.
            instanc.transform.SetParent(null);
            Destroy(instanc, 2f);
        }

        if (gameObject.tag == "Player")
        {
            PlayerController.animator.SetBool(AnimString.Instance.isAttack, false);     // ��ų �� ������ �Ҷ������� true�����ذ� false�� ��ȯ���־� ������ ������.
            PlayerController.animator.SetInteger(AnimString.Instance.attackStats, -1);  // �÷��̾ ������ ���ϸ� ���� ���ΰ��� �ʱ�ȭ �����־���.
        }
    }

    // ����
    public virtual void OnDeath()
    {
        if (isDeath) 
            return;

        if(CurrentHealth <= 0)
        {
            isDeath = true;
            // ���� ���� �ִϸ��̼��̶���� ���̴��� Ȱ���Ͽ�
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            if(animator != null)
            {
                animator.SetTrigger(AnimString.Instance.isDie);
                Debug.Log("����");
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

    IEnumerator AttBufPotion(Item item) // ���������� 
    {
        attack.AddValue(item.value);
        yield return new WaitForSecondsRealtime(item.buffTime);
        attack.RemoveValue(item.value);
    }

    IEnumerator DefBufPotion(Item item) // �� ������
    {
        defence.AddValue(item.value);
        yield return new WaitForSecondsRealtime(item.buffTime);
        defence.RemoveValue(item.value);
    }
}
