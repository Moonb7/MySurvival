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

        if ((targeMaskValue.value & (1 << otherLayer)) != 0) // targetLayerMaskValue�� �浹�ϴ� ���̾ �Ǵ�
        {
            totalDamage = stats.attack.GetValue() + WeaponManager.activeWeapon.weaponScriptable.atk; // ĳ������ ���� ���ݰ� ���� �������� ������ ���ݷ��� ���� ��

            damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
            }

            damageable.damageMultiplier = WeaponManager.activeWeapon.AttackStatedamageMultiplier();  // ���ݻ��¿� ���� ������ ��� ��ȭ
            damageable.InflictDamage(totalDamage, false, this.gameObject);
            //this.enabled= false;
        }
    }
}