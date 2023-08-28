using UnityEngine;

public class PlayerProjectileStandard : MonoBehaviour
{
    private Collider projectileCollider;

    public GameObject Owner { get; private set; } // ���μ���
    public LayerMask targetLayer;                 // Ÿ�Ϸ��̾� ����

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
        Owner = GameObject.FindGameObjectWithTag("Player");     // �ӽ� ����
        stats = Owner.GetComponentInParent<CharacterStats>();   // �ӽ� ����
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

        if ((targeMaskValue.value & (1 << otherLayer)) != 0) // targetLayerMaskValue�� �浹�ϴ� ���̾ �Ǵ�
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

        int otherLayer = other.layer;
        LayerMask targeMaskValue = targetLayer;

        if ((targeMaskValue.value & (1 << otherLayer)) != 0) // targetLayerMaskValue�� �浹�ϴ� ���̾ �Ǵ�
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
        Debug.Log(other.gameObject.name);
    }
}
