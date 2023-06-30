using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stats maxHealth;
    public float CurrentHealth { get; private set; }
    public Stats maxMana;
    public float CurrentMana { get; private set; }

    public Stats attack;
    public Stats defense;

    public bool isDeath = false;
    [SerializeField]
    private float DeathDelay = 3f;

    private Animator animator;

    [Tooltip("����")]
    public bool Invincible { get; set; }

    // �� �������� ������ �ִ��� üũ
    public bool CanPickUP() => CurrentHealth < maxHealth.GetValue();

    void SetStats()
    {
        CurrentHealth = maxHealth.GetValue();
        CurrentMana = maxMana.GetValue();
    }
    private void Start()
    {
        SetStats();

        animator = GetComponent<Animator>();
    }

    public void Heal(float amount)
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

    public void TakeDamage(float damage)
    {
        // �����̸� ������ ó��X
        if (Invincible)
            return;

        float beforeHealth = CurrentHealth;
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth.GetValue());

        // real Damage���ϱ� ������ �Ծ����� Ȯ��
        float realDamageAcount = beforeHealth - CurrentHealth;
        if(realDamageAcount > 0)
        {
            // ������ ���� � �����̳� �¾������� ȿ��

            if(animator != null)
            {
                animator.SetTrigger(AnimString.Instance.hit);
            }

        }
        OnDeath();
    }

    // �������
    public void UseMana(float amount)
    {
        float beforeMana = CurrentMana;
        CurrentMana -= amount;
        CurrentMana = Mathf.Clamp(CurrentMana, 0, maxMana.GetValue());

        // real Mana���ϱ� ������ �Ծ����� Ȯ��
        float realDamageAcount = beforeMana - CurrentMana;
        if (realDamageAcount > 0)
        {
            //
        }
    }

    // ����
    void OnDeath()
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
    
}
