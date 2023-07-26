using UnityEngine;

public class ProjectileStandard : MonoBehaviour
{
    private Collider projectileCollider;

    public GameObject Owner { get; private set; } // ���μ���

    private float totalAttack = 0;
    private CharacterStats stats;
    private Damageable damageable;

    private void OnEnable()
    {
        projectileCollider = GetComponent<Collider>();
        if(projectileCollider == null)
        {
            projectileCollider = null;
        }
        Owner = GameObject.FindGameObjectWithTag("Player");
        stats = Owner.GetComponentInParent<CharacterStats>();
        if (stats == null)
            stats = Owner.GetComponentInParent<CharacterStats>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (projectileCollider == null)
            return;

        if (stats.isDeath)
            return;

        if (other.gameObject.tag == "Enemy")
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
    private void OnParticleCollision(GameObject other)
    {
        if (projectileCollider != null)
            return;

        if (stats.isDeath)
            return;

        if (other.gameObject.tag == "Enemy")
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
