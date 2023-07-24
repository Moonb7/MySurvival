using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 애니메이션 이벤트에 쓸 함수를 관리 하는 클래스
public class HandleAnimationEvents : MonoBehaviour
{
    public Transform effectGenerator; // 이펙트 생성위치

    void FinishedRoll() // 구르기 애니매이션 끝날때쯤 이벤트 함수를 썻다
    {
        PlayerController.hasroll = false;
    }

    void FinishAttack() // 공격끝난 시점 처리
    {
        PlayerController.animator.SetBool(AnimString.Instance.isAttack, false);
        PlayerController.animator.SetBool(AnimString.Instance.chargingAtk, false);
    }

    void AttackEffectInstantiate() // 기본 공격 이펙트 및 사운드도 실행
    {
        GameObject instance = Instantiate(WeaponManager.activeWeapon.attackEffect); // 기본 공격 이펙트오브젝트 생성
        instance.transform.localPosition = effectGenerator.transform.position;
        instance.transform.localRotation = effectGenerator.transform.rotation;
        instance.transform.localScale = effectGenerator.transform.localScale;
        Destroy(instance, 5f);

        WeaponManager.activeWeapon.weaponAudioSource.clip = WeaponManager.activeWeapon.attackSound;
        WeaponManager.activeWeapon.weaponAudioSource.Play();
    }

    void ChargingAttackEffectInstantiate() // 기본 공격 이펙트 및 사운드도 실행
    {
        GameObject instance = Instantiate(WeaponManager.activeWeapon.chargingAttackEffect); // 기본 공격 이펙트오브젝트 생성
        instance.transform.localPosition = effectGenerator.transform.position;
        instance.transform.localRotation = effectGenerator.transform.rotation;
        instance.transform.localScale = effectGenerator.transform.localScale;
        Destroy(instance, 5f);

        WeaponManager.activeWeapon.weaponAudioSource.clip = WeaponManager.activeWeapon.chargingAttackSound;
        WeaponManager.activeWeapon.weaponAudioSource.Play();
    }

}
