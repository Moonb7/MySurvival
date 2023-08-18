using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWeapon : WeaponBase
{
    public override void Attack()   // �⺻����
    {
        AttackSetStats(AttackState.attack);

        startDamageMultiplier = 1;
        if (attackState == AttackState.attack)
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
        switch (attackState)
        {
            case AttackState.attack:
                if (comboCount == 3)
                {
                    damageMultiplier = 1.3f;
                }
                else
                {
                    damageMultiplier = startDamageMultiplier;
                }
                break;
            case AttackState.chargingAttack:
                damageMultiplier = 1.5f;
                break;
            case AttackState.skill1:

                break;
            case AttackState.skill2:
                damageMultiplier = 1.7f;
                break;
        }
        return damageMultiplier;
    }
}
