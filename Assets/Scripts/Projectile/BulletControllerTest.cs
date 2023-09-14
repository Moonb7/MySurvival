using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

public class BulletControllerTest : MonoBehaviour
{
    public float speed = 20f;
    public float sphereRadius = 0.1f; // 구체의 반지름

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
        // 초기 속도 부여
        velocity = transform.forward * speed;
    }

    void Update()
    {
        if (transform.parent != null)
        {
            return;
        }
            // 중력 적용
        velocity += gravity * Time.deltaTime * Time.deltaTime;

        // 총알의 이동에 따라 구체 충돌 판정 수행
        RaycastHit hit;
        if (Physics.SphereCast(arrowhead.position, sphereRadius, velocity.normalized, out hit, velocity.magnitude * Time.deltaTime))
        {
            // 충돌한 객체에 대한 처리
            // 예를 들어 충돌한 객체에 대한 데미지 적용 등을 할 수 있습니다.
            if (hit.transform.CompareTag("Player"))
            {
                totalDamage = stats.attack.GetValue(); // 캐릭터의 공격 스텟

                Damageable damageable = hit.transform.GetComponent<Damageable>();
                if (damageable == null)
                {
                    damageable = hit.transform.GetComponentInParent<Damageable>();
                    if (damageable == null) // 그럼에도 널이면 없는 것이니 그냥 실행시키지 말자
                        return;
                }
                damageable.InflictDamage(totalDamage, false, stats.gameObject);
                //this.enabled= false;
                Destroy(gameObject);
            }
        }
        // 총알 이동
        transform.position += velocity * Time.deltaTime;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(arrowhead.position, sphereRadius);
    }
}
