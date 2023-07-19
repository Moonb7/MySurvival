using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : WeaponBase
{
    public override void Attack()   // �⺻����
    {
        comboCount++;
        if (comboCount >= 4)
        {
            comboCount = 1;
        }
        PlayerController.animator.SetInteger(AnimString.Instance.combo, comboCount);
        PlayerController.animator.SetTrigger(AnimString.Instance.attack);       // �⺻ ���� �ִϸ��̼� ����
        PlayerController.animator.SetBool(AnimString.Instance.isAttack, true);  // ���� �� üũ �ִϸ��̼ǽ�ũ��Ʈ�� �̿��ؼ� false�������.
        
    }

    public override void ChargingAttack()
    {
        Debug.Log("��¡�����̿�");
    }

    public override void DashAttack()
    {
        // �ִ�
    }

    public override void Skill1()
    {
        // �ִ�
    }

    public override void Skill2()
    {
        // �ִ�
    }

    public override void UltimateSkill()
    {
        // �ִ�
    }
}
