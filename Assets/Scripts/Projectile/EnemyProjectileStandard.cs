using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyProjectileStandard : MonoBehaviour
{
    private Collider projectileCollider;
    private Rigidbody rb;

    public string targetTag;                       // ���� ��ų �� �����ϱ�
    public float speed;                            // ���ư� �ӵ�
    public float rotatSpeed;                       // ȸ����ų �ӵ�
    public float destoryDelay;

    public bool rotatProjectile;
    public bool scaleUpProjectile;
    public GameObject rotatObject;                 // ȸ���� ��ü

    private float totalDamage = 0;
    private CharacterStats stats;

    private Vector3 scale;
    private bool isReadyScale = false;
    private Vector3 rotation;

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

        if (scaleUpProjectile)
        {
            StartCoroutine(ScaleUp());
        }

        Destroy(gameObject, destoryDelay);
    }
    private void Update()
    {
        if (transform.parent == null) // ������ �θ𿡼� ���� ������ ���� ���� �����Ͽ��� ���⵵ �θ� �Ĵٺ��� �������� ���� �س���. // ������ Ÿ���� ���ؼ� �ѹ� ���ư��� ����
        {
            rb.velocity = transform.forward * speed * Time.deltaTime; // ������ ���� �־� ���� ���� ������� ���ư� ������ �ִϸ��̼��� ������ �����Ͽ� �����ߴ�.
        }

        Rotation();
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (projectileCollider == null)
            return;


        if (other.gameObject.CompareTag(targetTag))
        {
            totalDamage = stats.attack.GetValue(); // ĳ������ ���� ����

            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
                if (damageable == null) // �׷����� ���̸� ���� ���̴� �׳� �����Ű�� ����
                    return;
            }
            damageable.InflictDamage(totalDamage, false, stats.gameObject);
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
            totalDamage = stats.attack.GetValue(); // ĳ������ ���� ����

            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
                if (damageable == null) // �׷����� ���̸� ���� ���̴� �׳� �����Ű�� ����
                    return;
            }

            damageable.InflictDamage(totalDamage, false, stats.gameObject);
            //this.enabled= false;
            Destroy(gameObject);
        }
    }

    private IEnumerator ScaleUp() // ������ ���� �������� �÷� ũ�⸦ ������ϴ� �Լ� ���� �߻�ü�� �̿��� �����̴�
    {
        if (!isReadyScale)
        {
            while (!isReadyScale)
            {
                scale.x += 1f * Time.deltaTime;
                scale.y += 1f * Time.deltaTime;
                scale.z += 1f * Time.deltaTime;
                if (scale.x >= 1) // �������� ���� ������� ���ƿ���
                {
                    scale.x = 1;
                    scale.y = 1;
                    scale.z = 1;

                    isReadyScale = true;
                }
                transform.localScale = scale;
                if (isReadyScale)
                {
                    break;
                }
                yield return new WaitForSeconds(0.015f);
            }
        }
    }

    private void Rotation() // ȸ��
    {
        if (rotatProjectile)
        {
            rotation.x = rotatSpeed * Time.deltaTime;
            rotation.y = rotatSpeed * Time.deltaTime;
            rotation.z = rotatSpeed * Time.deltaTime;

            // 360�� �̻��̸� �ʱ�ȭ
            if (rotation.x >= 360f)
                rotation.x -= 360f;
            if (rotation.y >= 360f)
                rotation.y -= 360f;
            if (rotation.z >= 360f)
                rotation.z -= 360f;

            rotatObject.transform.Rotate(rotation.x, rotation.y, rotation.z);
        }
    }
}
