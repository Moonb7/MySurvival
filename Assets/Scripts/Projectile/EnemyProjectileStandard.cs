using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileStandard : MonoBehaviour
{
    private Collider projectileCollider;
    private Rigidbody rb;

    public string targetTag;                       // 적중 시킬 적 설정하기
    public float speed;                            // 날아갈 속도

    private float totalAttack = 0;
    private CharacterStats stats;
    private Damageable damageable;

    private void Start()
    {
        // 처음 생성될떄는 Enemy의 자식오브젝트로 만들고 애니메이션의 화살을 쏘는 시점에서 자식오브젝트말고 외부로 빠져 날아가게 만들예정이다.
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
        // 인제 날라가게
        if (transform.parent == null)
        {
            rb.velocity = transform.forward * speed * Time.deltaTime; // 앞으로 힘을 주어 날아 가게 만들었고 날아갈 방향은 애니메이션의 방향을 조절하여 설정했다.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (projectileCollider == null)
            return;


        if (other.gameObject.CompareTag(targetTag))
        {
            totalAttack = stats.attack.GetValue(); // 캐릭터의 공격 스텟

            damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
                if (damageable == null) // 그럼에도 널이면 없는 것이니 그냥 실행시키지 말자
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
            totalAttack = stats.attack.GetValue(); // 캐릭터의 공격 스텟

            damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
                if (damageable == null) // 그럼에도 널이면 없는 것이니 그냥 실행시키지 말자
                    return;
            }

            damageable.InflictDamage(totalAttack, false, stats.gameObject);
            //this.enabled= false;
            Destroy(gameObject);
        }
    }
}
