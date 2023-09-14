using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

public class BulletControllerTest : MonoBehaviour
{
    public float speed = 20f;
    public float sphereRadius = 0.1f; // ��ü�� ������

    private Vector3 velocity;
    private Vector3 gravity = Physics.gravity;

    public Transform arrowhead;
    public float destoryDelay;

    private float totalDamage;
    private CharacterStats stats;

    void Start()
    {
        stats = GetComponent<CharacterStats>();
        if (stats == null)
            stats = GetComponentInParent<CharacterStats>();

        Destroy(gameObject, destoryDelay);
        // �ʱ� �ӵ� �ο�
        velocity = transform.forward * speed;
    }

    void Update()
    {
        if (transform.parent != null)
        {
            return;
        }
            // �߷� ����
        velocity += gravity * Time.deltaTime * Time.deltaTime;

        // �Ѿ��� �̵��� ���� ��ü �浹 ���� ����
        RaycastHit hit;
        if (Physics.SphereCast(arrowhead.position, sphereRadius, velocity.normalized, out hit, velocity.magnitude * Time.deltaTime))
        {
            // �浹�� ��ü�� ���� ó��
            // ���� ��� �浹�� ��ü�� ���� ������ ���� ���� �� �� �ֽ��ϴ�.
            if (hit.transform.CompareTag("Player"))
            {
                totalDamage = stats.attack.GetValue(); // ĳ������ ���� ����

                Damageable damageable = hit.transform.GetComponent<Damageable>();
                if (damageable == null)
                {
                    damageable = hit.transform.GetComponentInParent<Damageable>();
                    if (damageable == null) // �׷����� ���̸� ���� ���̴� �׳� �����Ű�� ����
                        return;
                }
                damageable.InflictDamage(totalDamage, false, stats.gameObject);
                //this.enabled= false;
                Destroy(gameObject);
            }
        }
        // �Ѿ� �̵�
        transform.position += velocity * Time.deltaTime;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(arrowhead.position, sphereRadius);
    }
}
