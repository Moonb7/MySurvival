using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon2 : WeaponBase
{
    public override void Attack()   // 기본공격
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
        // 카메라 흔들림도 고려해보기
    }

    public override void Skill1() // 버프 스킬
    {
        AttackSetStats(AttackState.skill1);

        StartCoroutine(PowerUpSkill1());
        CreateBuff();
    }

    IEnumerator PowerUpSkill1()
    {
        characterStats.attack.AddValue(weaponScriptable.buffValue);             // 일단 이렇게 테스트로 하고 스크랩터블오브젝트에 변수 생성할지 고민하자
        yield return new WaitForSecondsRealtime(weaponScriptable.buffTime);     // 지속시간
        characterStats.attack.RemoveValue(weaponScriptable.buffValue);          // 다시 없애기
    }
    public bool CreateBuff() // 인풋 스킬에다 생성
    {
        SkillUI skillUI = GameObject.Find("UIManager").GetComponent<SkillUI>(); // 스킬UI스크립트에서 statusEffect위치를 찾아 생성하였다.
        if (WeaponManager.activeWeapon.buffImage != null)
        {
            return Instantiate(WeaponManager.activeWeapon.buffImage, skillUI.statusEffect);
        }
        else
        {
            return false;
        }
    }

    public override void Skill2() // 공격 스킬
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
