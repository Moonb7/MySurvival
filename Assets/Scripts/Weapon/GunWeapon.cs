using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWeapon : WeaponBase
{
    public override void Attack()
    {
        
    }
    public override void ChargingAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void Skill1()
    {
        throw new System.NotImplementedException();
    }

    public override void Skill2()
    {
        throw new System.NotImplementedException();
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
                damageMultiplier = 1.7f;
                break;
            case AttackState.skill2:
                break;
        }
        return damageMultiplier;
    }
}
