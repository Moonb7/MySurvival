using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float totalAttack = 0;

    private CharacterStats stats;
    

    private Damageable damageable;

    private void Start()
    {
        stats = GetComponentInParent<CharacterStats>();
        if(stats == null )
            stats = GetComponentInParent<CharacterStats>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (stats.isDeath)
            return;

        if(other.gameObject.tag == "Enemy")
        {
            totalAttack = stats.attack.GetValue() + WeaponManager.activeWeapon.weaponScriptable.atk; // ĳ������ ���� ���ݰ� ���� �������� ������ ���ݷ��� ���� ��

            damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
            }
            WeaponManager.activeWeapon.multiplierDamage = 0 >= WeaponManager.activeWeapon.multiplierDamage ? 1 : WeaponManager.activeWeapon.multiplierDamage;
            damageable.InflictDamage(totalAttack * WeaponManager.activeWeapon.multiplierDamage, false, this.gameObject);
            this.enabled= false;
        }
    }
}