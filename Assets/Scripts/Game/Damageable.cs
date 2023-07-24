using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 데미지를 입는 클래스
public class Damageable : MonoBehaviour
{
    private CharacterStats characterStats;
    public float damageMultiplier = 1.0f; // 데미지 계수
    public float sensibilityToSelfDamage = 0.5f; // 자신이 공격한 범위 폭발공격을 데미지입을시 자신이 입는 데미지가 완전히 입지 않게 해주기 위한 변수

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        if(characterStats == null)
        {
            characterStats = GetComponentInParent<CharacterStats>();
        }
    }

    public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
    {
        if(characterStats != null)
        {
            var totalDamage = damage;

            // 폭발 데미지 체크 - 폭발 데미지 일때는 damageMultiplier 생략 범위별로 나타내 데미지를 주기위해
            if (isExplosionDamage == false)
            {
                totalDamage *= damageMultiplier;
            }

            // 자신이 입힌 데미지이면 내가 공격해서 입는 데미지
            if(characterStats.gameObject == damageSource)
            {
                totalDamage *= sensibilityToSelfDamage;
            }
            // 데미지 계산
            characterStats.TakeDamage(totalDamage);
            Debug.Log(characterStats.CurrentHealth);
        }
    }

}
