using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float totalDamage = 0;
    private CharacterStats stats;
    private Damageable damageable;

    public LayerMask enemyLayer;

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

        int otherLayer = other.gameObject.layer;
        LayerMask targeMaskValue = enemyLayer;

        if ((targeMaskValue.value & (1 << otherLayer)) != 0) // targetLayerMaskValue와 충돌하는 레이어를 판단
        {
            totalDamage = stats.attack.GetValue() + WeaponManager.activeWeapon.weaponScriptable.atk; // 캐릭터의 공격 스텟과 현재 장착중인 무기의 공격력을 더한 값

            damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
            }

            damageable.damageMultiplier = WeaponManager.activeWeapon.AttackStatedamageMultiplier();  // 공격상태에 따라 데미지 계수 강화
            damageable.InflictDamage(totalDamage, false, this.gameObject);
            //this.enabled= false;
        }
    }
}