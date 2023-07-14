using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // 나중에 무기 공격력을 합치든 이걸 정하든 설정하자
    private float totalAttack = 0;

    private CharacterStats stats;
    private WeaponBase weapon;

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
            totalAttack = stats.attack.GetValue() + WeaponManager.activeWeapon.weaponScriptable.atk; // 캐릭터의 공격 스텟과 현재 장착중인 무기의 공격력을 더한 값

            damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
            }
            damageable.InflictDamage(totalAttack, false, this.gameObject);
            this.enabled= false;
        }
    }
}