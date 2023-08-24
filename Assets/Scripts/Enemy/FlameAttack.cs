using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlameAttack : MonoBehaviour
{
    public CharacterStats stats;

    public LayerMask targetMask;
    public float damageTime;
    private float countDown;

    private void Update()
    {
        if (countDown > 0)
            countDown -= Time.deltaTime;
    }

    private void OnParticleCollision(GameObject other)
    {
        int otherLayer = other.layer;
        LayerMask targeMaskValue = targetMask;

        float totalDamage = stats.attack.GetValue(); // ĳ������ ���� ����

        if ((targeMaskValue.value & (1 << otherLayer)) != 0) // targetLayerMaskValue�� �浹�ϴ� ���̾ �Ǵ�
        {
            if(countDown <= 0)
            {
                countDown = damageTime;

                Damageable damageable = other.GetComponent<Damageable>();
                if (damageable == null)
                {
                    damageable = other.GetComponentInParent<Damageable>();
                    if (damageable == null) // �׷����� ���̸� ���� ���̴� �׳� �����Ű�� ����
                        return;
                }

                damageable.InflictDamage(totalDamage, false, stats.gameObject);
            }
        }
    }
}
