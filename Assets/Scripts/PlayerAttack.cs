using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    // ���߿� ���� ���ݷ��� ��ġ�� �̰� ���ϵ� ��������
    private float totalAttack = 0;

    private CharacterStats stats;
    private WeaponController weapon;

    private Damageable damageable;

    private void Start()
    {
        stats = GetComponent<CharacterStats>();
        if(stats == null )
            stats = GetComponentInParent<CharacterStats>();
        weapon= GetComponent<WeaponController>();
        if(weapon == null )
        weapon = GetComponentInParent<WeaponController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (stats.isDeath)
            return;

        if(other.gameObject.tag == "Enemy")
        {
            totalAttack = stats.attack.GetValue() + weapon.Atk;

            damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
            }
            damageable.InflictDamage(totalAttack, false, this.gameObject);
        }
    }
}