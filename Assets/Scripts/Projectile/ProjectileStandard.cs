using UnityEngine;

public class ProjectileStandard : MonoBehaviour
{
    private Collider projectileCollider;

    public GameObject Owner { get; private set; } // ���μ���
    public string targetTag;                      //���� ��ų �� �����ϱ�

    private float totalAttack = 0;
    private CharacterStats stats;
    private Damageable damageable;

    // --------------


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


        if (other.gameObject.CompareTag(targetTag))
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

        if (other.gameObject.CompareTag(targetTag))
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
