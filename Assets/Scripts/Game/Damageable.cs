using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� �Դ� Ŭ����
public class Damageable : MonoBehaviour
{
    private CharacterStats characterStats;
    public float damageMultiplier = 1.0f; // ������ ���
    public float sensibilityToSelfDamage = 0.5f; // �ڽ��� ������ ���� ���߰����� ������������ �ڽ��� �Դ� �������� ������ ���� �ʰ� ���ֱ� ���� ����

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        if(characterStats == null)
        {
            characterStats = GetComponentInParent<CharacterStats>();
        }
    }

    public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
    {
        if(characterStats != null)
        {
            var totalDamage = damage;

            // ���� ������ üũ - ���� ������ �϶��� damageMultiplier ���� �������� ��Ÿ�� �������� �ֱ�����
            if (isExplosionDamage == false)
            {
                totalDamage *= damageMultiplier;
            }

            // �ڽ��� ���� �������̸� ���� �����ؼ� �Դ� ������
            if(characterStats.gameObject == damageSource)
            {
                totalDamage *= sensibilityToSelfDamage;
            }
            // ������ ���
            characterStats.TakeDamage(totalDamage);
            Debug.Log(characterStats.CurrentHealth);
        }
    }

}
