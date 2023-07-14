using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 애니메이션 이벤트에 쓸 함수를 관리 하는 클래스
public class HandleAnimationEvents : MonoBehaviour
{
    public Transform effectGenerator; // 이펙트 생성위치

    // 구르기 애니매이션 끝날때쯤 이벤트 함수를 썻다
    void FinishedRoll()
    {
        PlayerNewInputController.hasroll = false;
    }

    void InstantiateEffect() // 이펙트 생성
    {
        GameObject instance = Instantiate(WeaponManager.activeWeapon.attackEffect, effectGenerator); // 기본 공격 이펙트오브젝트 생성
        Destroy(instance, 5f);
    }
}
