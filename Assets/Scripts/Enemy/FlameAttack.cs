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
    private ParticleSystem ps;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (countDown > 0)
            countDown -= Time.deltaTime;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (stats.isDeath)
        {
            var mainModule = ps.main; // 파티클 시스템의 main 모듈을 가져옴
            mainModule.loop = false; // loop 속성 설정
            return;
        }

        int otherLayer = other.layer;
        LayerMask targeMaskValue = targetMask;

        float totalDamage = stats.attack.GetValue(); // 캐릭터의 공격 스텟

        if ((targeMaskValue.value & (1 << otherLayer)) != 0) // targetLayerMaskValue와 충돌하는 레이어를 판단
        {
            if(countDown <= 0)
            {
                countDown = damageTime;

                Damageable damageable = other.GetComponent<Damageable>();
                if (damageable == null)
                {
                    damageable = other.GetComponentInParent<Damageable>();
                    if (damageable == null) // 그럼에도 널이면 없는 것이니 그냥 실행시키지 말자
                        return;
                }

                damageable.InflictDamage(totalDamage, false, stats.gameObject);
            }
        }
    }
}
