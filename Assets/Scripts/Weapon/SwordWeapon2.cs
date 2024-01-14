using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon2 : WeaponBase
{
    public override void Attack()   // �⺻����
    {
        AttackSetStats(AttackState.attack);

        if (AttackState == AttackState.attack)
        {
            comboCount++;
            if (comboCount >= 4)
            {
                comboCount = 1;
            }
        }
    }

    public override void ChargingAttack()
    {
        AttackSetStats(AttackState.chargingAttack);
        // ī�޶� ��鸲�� ����غ���
    }

    public override void Skill1() // ���� ��ų
    {
        AttackSetStats(AttackState.skill1);

        StartCoroutine(PowerUpSkill1());
        CreateBuff();
    }

    IEnumerator PowerUpSkill1()
    {
        characterStats.attack.AddValue(weaponScriptable.buffValue);             // �ϴ� �̷��� �׽�Ʈ�� �ϰ� ��ũ���ͺ������Ʈ�� ���� �������� �������
        yield return new WaitForSecondsRealtime(weaponScriptable.buffTime);     // ���ӽð�
        characterStats.attack.RemoveValue(weaponScriptable.buffValue);          // �ٽ� ���ֱ�
    }
    public bool CreateBuff() // ��ǲ ��ų���� ����
    {
        SkillUI skillUI = GameObject.Find("UIManager").GetComponent<SkillUI>(); // ��ųUI��ũ��Ʈ���� statusEffect��ġ�� ã�� �����Ͽ���.
        if (WeaponManager.activeWeapon.buffImage != null)
        {
            return Instantiate(WeaponManager.activeWeapon.buffImage, skillUI.statusEffect);
        }
        else
        {
            return false;
        }
    }

    public override void Skill2() // ���� ��ų
    {
        AttackSetStats(AttackState.skill2);
    }

    public override float AttackStatedamageMultiplier()
    {
        switch (AttackState)
        {
            case AttackState.attack:
                if (comboCount == 3)
                {
                    DamageMultiplier = 1.3f;
                }
                else
                {
                    DamageMultiplier = normalAttackDamageMultiplier;
                }
                break;
            case AttackState.chargingAttack:
                DamageMultiplier = chagingAttackDamageMultiplier;
                break;
            case AttackState.skill1:
                DamageMultiplier = skill1AttackDamageMultiplier;
                break;
            case AttackState.skill2:
                DamageMultiplier = skill2AttackDamageMultiplier;
                break;
        }
        return DamageMultiplier;
    }
}
