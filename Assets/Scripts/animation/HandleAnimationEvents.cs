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
        PlayerController.animator.SetInteger(AnimString.Instance.attackStats, -1);
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

    void Skill1EffectInstantiate() // �⺻ ���� ����Ʈ �� ���嵵 ����
    {
        GameObject instance = Instantiate(WeaponManager.activeWeapon.skill1Effect); // �⺻ ���� ����Ʈ������Ʈ ����
        instance.transform.localPosition = effectGenerator.transform.position;
        instance.transform.localRotation = effectGenerator.transform.rotation;
        instance.transform.localScale = effectGenerator.transform.localScale;
        Destroy(instance, 5f);

        WeaponManager.activeWeapon.weaponAudioSource.clip = WeaponManager.activeWeapon.skill1Sound;
        WeaponManager.activeWeapon.weaponAudioSource.Play();
    }

    void Skill2EffectInstantiate() // �⺻ ���� ����Ʈ �� ���嵵 ����
    {
        GameObject instance = Instantiate(WeaponManager.activeWeapon.skill2Effect); // �⺻ ���� ����Ʈ������Ʈ ����
        instance.transform.localPosition = effectGenerator.transform.position;
        instance.transform.localRotation = effectGenerator.transform.rotation;
        instance.transform.localScale = effectGenerator.transform.localScale;
        Destroy(instance, 5f);

        WeaponManager.activeWeapon.weaponAudioSource.clip = WeaponManager.activeWeapon.skill2Sound;
        WeaponManager.activeWeapon.weaponAudioSource.Play();
    }
}
