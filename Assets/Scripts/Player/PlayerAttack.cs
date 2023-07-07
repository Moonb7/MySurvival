using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // ���߿� ���� ���ݷ��� ��ġ�� �̰� ���ϵ� ��������
    private float totalAttack = 0;

    private CharacterStats stats;
    private WeaponBase weapon;

    private Damageable damageable;

    private void Start()
    {
        stats = GetComponent<CharacterStats>();
        if(stats == null )
            stats = GetComponentInParent<CharacterStats>();
        weapon= GetComponent<WeaponBase>();
        if(weapon == null )
        weapon = GetComponentInParent<WeaponBase>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (stats.isDeath)
            return;

        if(other.gameObject.tag == "Enemy")
        {
            totalAttack = stats.attack.GetValue() + weapon.weaponScriptable.atk;

            damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
            }
            damageable.InflictDamage(totalAttack, false, this.gameObject);
        }
    }
}