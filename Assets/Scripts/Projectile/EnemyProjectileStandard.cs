using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileStandard : MonoBehaviour
{
    private Collider projectileCollider;
    private Rigidbody rb;

    public string targetTag;                       // ���� ��ų �� �����ϱ�
    public float speed;                            // ���ư� �ӵ�

    private float totalAttack = 0;
    private CharacterStats stats;
    private Damageable damageable;

    private void Start()
    {
        // ó�� �����ɋ��� Enemy�� �ڽĿ�����Ʈ�� ����� �ִϸ��̼��� ȭ���� ��� �������� �ڽĿ�����Ʈ���� �ܺη� ���� ���ư��� ���鿹���̴�.
        projectileCollider = GetComponent<Collider>();
        if (projectileCollider == null)
        {
            projectileCollider = null;
        }

        stats = GetComponent<CharacterStats>();   
        if (stats == null)
            stats = GetComponentInParent<CharacterStats>();

        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        // ���� ���󰡰�
        if (transform.parent == null)
        {
            rb.velocity = transform.forward * speed * Time.deltaTime; // ������ ���� �־� ���� ���� ������� ���ư� ������ �ִϸ��̼��� ������ �����Ͽ� �����ߴ�.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (projectileCollider == null)
            return;


        if (other.gameObject.CompareTag(targetTag))
        {
            totalAttack = stats.attack.GetValue(); // ĳ������ ���� ����

            damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
                if (damageable == null) // �׷����� ���̸� ���� ���̴� �׳� �����Ű�� ����
                    return;
            }
            damageable.InflictDamage(totalAttack, false, stats.gameObject);
            //this.enabled= false;
            Destroy(gameObject);
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
            totalAttack = stats.attack.GetValue(); // ĳ������ ���� ����

            damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
                if (damageable == null) // �׷����� ���̸� ���� ���̴� �׳� �����Ű�� ����
                    return;
            }

            damageable.InflictDamage(totalAttack, false, stats.gameObject);
            //this.enabled= false;
            Destroy(gameObject);
        }
    }
}
