using UnityEngine;

public class PlayerProjectileStandard : MonoBehaviour
{
    private Collider projectileCollider;

    public GameObject Owner { get; private set; } // 주인설정
    public LayerMask targetLayer;                 // 타켓레이어 설정

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
        Owner = GameObject.FindGameObjectWithTag("Player");     // 임시 방편
        stats = Owner.GetComponentInParent<CharacterStats>();   // 임시 방편
        if (stats == null)
            stats = Owner.GetComponentInParent<CharacterStats>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (projectileCollider == null)
            return;

        if (stats.isDeath)
            return;

        int otherLayer = other.gameObject.layer;
        LayerMask targeMaskValue = targetLayer;

        if ((targeMaskValue.value & (1 << otherLayer)) != 0) // targetLayerMaskValue와 충돌하는 레이어를 판단
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

        int otherLayer = other.layer;
        LayerMask targeMaskValue = targetLayer;

        if ((targeMaskValue.value & (1 << otherLayer)) != 0) // targetLayerMaskValue와 충돌하는 레이어를 판단
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
        Debug.Log(other.gameObject.name);
    }
}
