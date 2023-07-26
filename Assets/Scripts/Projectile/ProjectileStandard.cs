using UnityEngine;

public class ProjectileStandard : MonoBehaviour
{
    private Collider projectileCollider;

    public GameObject Owner { get; private set; } // 주인설정

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
            totalAttack = stats.attack.GetValue() + WeaponManager.activeWeapon.weaponScriptable.atk; // 캐릭터의 공격 스텟과 현재 장착중인 무기의 공격력을 더한 값

            damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
            }
            damageable.damageMultiplier = WeaponManager.activeWeapon.AttackStatedamageMultiplier();  // 공격상태에 따라 데미지 계수 강화
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
            totalAttack = stats.attack.GetValue() + WeaponManager.activeWeapon.weaponScriptable.atk; // 캐릭터의 공격 스텟과 현재 장착중인 무기의 공격력을 더한 값

            damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
            }

            damageable.damageMultiplier = WeaponManager.activeWeapon.AttackStatedamageMultiplier();  // 공격상태에 따라 데미지 계수 강화
            damageable.InflictDamage(totalAttack, false, this.gameObject);
            //this.enabled= false;
        }
    }
}
