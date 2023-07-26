using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : WeaponBase
{
    public override void Attack()   // 기본공격
    {
        startDamageMultiplier = 1;
        attackState = AttackState.attack;
        comboCount++;
        if (comboCount >= 4)
        {
            comboCount = 1;
        }
        PlayerController.animator.SetBool(AnimString.Instance.isAttack, true);  // 공격 중 체크 애니메이션스크립트를 이용해서 false만들었다.
    }

    public override void ChargingAttack()
    {
        attackState = AttackState.chargingAttack;
        PlayerController.animator.SetBool(AnimString.Instance.isAttack, true);
        PlayerController.animator.SetBool(AnimString.Instance.chargingAtk, true);
    }

    public override void Skill1() // 공격 스킬
    {
        attackState = AttackState.skill1;
        // 애니
    }

    public override void Skill2() // 버프 스킬
    {
        attackState = AttackState.skill2;
        // 애니
    }

    public override void UltimateSkill() //
    {
        attackState = AttackState.UltimataeSkill;
        // 애니
    }

    public override float AttackStatedamageMultiplier()
    {
        switch (attackState)
        {
            case AttackState.attack:
                if(comboCount == 3)
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
                damageMultiplier = 1.7f;
                break;
            case AttackState.skill2:
                break;
            case AttackState.UltimataeSkill:
                damageMultiplier = 3f;
                break;
        }
        return damageMultiplier;
    }
}
