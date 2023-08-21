using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletControllerTest : MonoBehaviour
{
    public float speed = 20f;
    public float sphereRadius = 0.1f; // ��ü�� ������

    private Vector3 velocity;
    private Vector3 gravity = Physics.gravity;

    public Transform arrowhead;

    void Start()
    {
        // �ʱ� �ӵ� �ο�
        velocity = transform.forward * speed;
    }

    void Update()
    {
        // �߷� ����
        velocity += gravity * Time.deltaTime;

        // �Ѿ��� �̵��� ���� ��ü �浹 ���� ����
        RaycastHit hit;
        if (Physics.SphereCast(arrowhead.position, sphereRadius, velocity.normalized, out hit, velocity.magnitude * Time.deltaTime))
        {
            // �浹�� ��ü�� ���� ó��
            // ���� ��� �浹�� ��ü�� ���� ������ ���� ���� �� �� �ֽ��ϴ�.
            
            Destroy(gameObject);
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
