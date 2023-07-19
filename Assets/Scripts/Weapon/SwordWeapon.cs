using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : WeaponBase
{
    public override void Attack()   // 기본공격
    {
        comboCount++;
        if (comboCount >= 4)
        {
            comboCount = 1;
        }
        PlayerController.animator.SetInteger(AnimString.Instance.combo, comboCount);
        PlayerController.animator.SetTrigger(AnimString.Instance.attack);       // 기본 공격 애니메이션 적용
        PlayerController.animator.SetBool(AnimString.Instance.isAttack, true);  // 공격 중 체크 애니메이션스크립트를 이용해서 false만들었다.
        
    }

    public override void ChargingAttack()
    {
        Debug.Log("차징공격이요");
    }

    public override void DashAttack()
    {
        // 애니
    }

    public override void Skill1()
    {
        // 애니
    }

    public override void Skill2()
    {
        // 애니
    }

    public override void UltimateSkill()
    {
        // 애니
    }
}
