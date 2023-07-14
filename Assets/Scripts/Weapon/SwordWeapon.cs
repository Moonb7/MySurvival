using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : WeaponBase
{
    public override void Attack()
    {
        PlayerNewInputController.animator.SetTrigger(AnimString.Instance.attack);       // �⺻ ���� �ִϸ��̼� ����
        PlayerNewInputController.animator.SetBool(AnimString.Instance.isAttack, true);  // ���� �� üũ �ִϸ��̼ǽ�ũ��Ʈ�� �̿��ؼ� false�������.
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
