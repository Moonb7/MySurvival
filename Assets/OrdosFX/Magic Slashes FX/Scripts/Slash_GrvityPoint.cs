using UnityEngine;

[ExecuteInEditMode]
public class Slash_GrvityPoint : MonoBehaviour
{
    private Transform Target;
    public float Force = 1;
    public float StopDistance = 0;
    ParticleSystem ps;
    ParticleSystem.Particle[] particles;

    ParticleSystem.MainModule mainModule;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        mainModule = ps.main;
        Target= transform;
    }

    void LateUpdate()
    {
        if (Target == null) return;
        var maxParticles = mainModule.maxParticles;
        if (particles == null || particles.Length < maxParticles)
        {
            particles = new ParticleSystem.Particle[maxParticles];
        }
        int particleCount = ps.GetParticles(particles);

        var targetTransformedPosition = Vector3.zero;
        if (mainModule.simulationSpace == ParticleSystemSimulationSpace.Local)
            targetTransformedPosition = transform.InverseTransformPoint(Target.position);
        if (mainModule.simulationSpace == ParticleSystemSimulationSpace.World)
            targetTransformedPosition = Target.position;


        float forceDeltaTime = Time.deltaTime * Force;

        for (int i = 0; i < particleCount; i++)
        {
            var distanceToParticle = targetTransformedPosition - particles[i].position;
           
            if (StopDistance > 0.001f && distanceToParticle.magnitude < StopDistance)
            {
                particles[i].velocity = Vector3.zero;
            }
            else
            {
                var directionToTarget = Vector3.Normalize(distanceToParticle);
                var seekForce = directionToTarget*forceDeltaTime;

                particles[i].velocity += seekForce;
            }
        }

        if(WeaponManager.activeWeapon.weaponScriptable.chargingEnergyTime <= InputManager.chargingEnergy)
        {
            var mainModule = ps.main; // 파티클 시스템의 main 모듈을 가져옴
            mainModule.loop = false;  // loop 속성 설정
        }

        ps.SetParticles(particles, particleCount);
    }
}
