using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyProjectileStandard : MonoBehaviour
{
    private Collider projectileCollider;
    private Rigidbody rb;

    public string targetTag;                       // 적중 시킬 적 설정하기
    public float speed;                            // 날아갈 속도
    public float rotatSpeed;                       // 회전시킬 속도
    public float destoryDelay;

    public bool rotatProjectile;
    public bool scaleUpProjectile;
    public GameObject rotatObject;                 // 회전할 물체

    private float totalDamage = 0;
    private CharacterStats stats;

    private Vector3 scale;
    private bool isReadyScale = false;
    private Vector3 rotation;

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

        if (scaleUpProjectile)
        {
            StartCoroutine(ScaleUp());
        }

        Destroy(gameObject, destoryDelay);
    }
    private void Update()
    {
        if (transform.parent == null) // 생성된 부모에서 빠져 나오면 날아 가게 설정하였고 방향도 부모가 쳐다보는 방향으로 설정 해놨다. // 이제는 타겟을 정해서 한번 날아가게 하자
        {
            rb.velocity = transform.forward * speed * Time.deltaTime; // 앞으로 힘을 주어 날아 가게 만들었고 날아갈 방향은 애니메이션의 방향을 조절하여 설정했다.
        }

        Rotation();
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (projectileCollider == null)
            return;


        if (other.gameObject.CompareTag(targetTag))
        {
            totalDamage = stats.attack.GetValue(); // 캐릭터의 공격 스텟

            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
                if (damageable == null) // 그럼에도 널이면 없는 것이니 그냥 실행시키지 말자
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
            totalDamage = stats.attack.GetValue(); // 캐릭터의 공격 스텟

            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<Damageable>();
                if (damageable == null) // 그럼에도 널이면 없는 것이니 그냥 실행시키지 말자
                    return;
            }

            damageable.InflictDamage(totalDamage, false, stats.gameObject);
            //this.enabled= false;
            Destroy(gameObject);
        }
    }

    private IEnumerator ScaleUp() // 생성시 점점 스케일을 늘려 크기를 맟출려하는 함수 보스 발사체를 이용할 예정이다
    {
        if (!isReadyScale)
        {
            while (!isReadyScale)
            {
                scale.x += 1f * Time.deltaTime;
                scale.y += 1f * Time.deltaTime;
                scale.z += 1f * Time.deltaTime;
                if (scale.x >= 1) // 스케일이 원래 모습으로 돌아오면
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

    private void Rotation() // 회전
    {
        if (rotatProjectile)
        {
            rotation.x = rotatSpeed * Time.deltaTime;
            rotation.y = rotatSpeed * Time.deltaTime;
            rotation.z = rotatSpeed * Time.deltaTime;

            // 360도 이상이면 초기화
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
