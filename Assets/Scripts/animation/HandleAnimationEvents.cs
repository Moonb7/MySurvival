using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ִϸ��̼� �̺�Ʈ�� �� �Լ��� ���� �ϴ� Ŭ����
public class HandleAnimationEvents : MonoBehaviour
{
    public Transform effectGenerator; // ����Ʈ ������ġ

    void FinishedRoll() // ������ �ִϸ��̼� �������� �̺�Ʈ �Լ��� ����
    {
        PlayerController.hasroll = false;
    }

    void FinishAttack() // ���ݳ��� ���� ó��
    {
        PlayerController.animator.SetBool(AnimString.Instance.isAttack, false);
        PlayerController.animator.SetBool(AnimString.Instance.chargingAtk, false);
    }

    void AttackEffectInstantiate() // �⺻ ���� ����Ʈ �� ���嵵 ����
    {
        GameObject instance = Instantiate(WeaponManager.activeWeapon.attackEffect); // �⺻ ���� ����Ʈ������Ʈ ����
        instance.transform.localPosition = effectGenerator.transform.position;
        instance.transform.localRotation = effectGenerator.transform.rotation;
        instance.transform.localScale = effectGenerator.transform.localScale;
        Destroy(instance, 5f);

        WeaponManager.activeWeapon.weaponAudioSource.clip = WeaponManager.activeWeapon.attackSound;
        WeaponManager.activeWeapon.weaponAudioSource.Play();
    }

    void ChargingAttackEffectInstantiate() // �⺻ ���� ����Ʈ �� ���嵵 ����
    {
        GameObject instance = Instantiate(WeaponManager.activeWeapon.chargingAttackEffect); // �⺻ ���� ����Ʈ������Ʈ ����
        instance.transform.localPosition = effectGenerator.transform.position;
        instance.transform.localRotation = effectGenerator.transform.rotation;
        instance.transform.localScale = effectGenerator.transform.localScale;
        Destroy(instance, 5f);

        WeaponManager.activeWeapon.weaponAudioSource.clip = WeaponManager.activeWeapon.chargingAttackSound;
        WeaponManager.activeWeapon.weaponAudioSource.Play();
    }

}
