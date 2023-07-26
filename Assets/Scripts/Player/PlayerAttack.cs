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

            damageable.damageMultiplier = WeaponManager.activeWeapon.AttackStatedamageMultiplier();  // ���ݻ��¿� ���� ������ ��� ��ȭ
            damageable.InflictDamage(totalAttack, false, this.gameObject);
            //this.enabled= false;
        }
    }
}