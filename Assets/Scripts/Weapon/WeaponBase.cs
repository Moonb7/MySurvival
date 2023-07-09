using UnityEditor.Animations;
using UnityEngine;

// 무기 오브젝트 프리팹에 넣을 컴포넌트 클래스이다. WeaponController 이다.
public abstract class WeaponBase : MonoBehaviour
{
    [Tooltip("무기 기본정보가 들어있는 무기 ScriptableObject")]
    public WeaponScriptable weaponScriptable;
    [Tooltip("해당 무기의 애니메이터컨트롤러")] // 이러면 해당 무기의 애니메이터를 만들어 플레이어의 컨트롤러를 교체만 해주면 간단하게 각 해당 무기의 애니매이션을 쉽게 조절 가능할 것 같다.
    public AnimatorController weaponAnimator;

    // 무기의 현재 콤보 카운터
    public int comboCount { get; set; }

    [Header("무기 공격 이펙트")]
    public GameObject attackEffect;         // 기본공격 이펙트
    public GameObject dashAttackEffect;     // 대쉬공격 이펙트
    public GameObject chargingEffect;       // 차징 중 이펙트
    public GameObject chargingAttackEffect; // 차지공격 이펙트
    public GameObject skill1Effect;         // 스킬1 이펙트
    public GameObject skill2Effect;         // 스킬2 이펙트
    public GameObject ultimateSkillEffect;  // 궁극기 이펙트

    [Header("무기 공격 사운드")]
    public AudioClip attackSound;           // 기본공격 소리
    public AudioClip dashAttackSound;       // 대쉬공격 소리
    public AudioClip chargingSound;         // 차징 중 소리
    public AudioClip chargingAttackSound;   // 차징공격 소리
    public AudioClip skill1Sound;           // 스킬1 소리
    public AudioClip skill2Sound;           // 스킬2 소리
    public AudioClip ultimateSkillSound;    // 궁국기 소리

    public abstract void Attack();          // 기본 공격
    public abstract void DashAttack();      // 대시 공격
    public abstract void ChargingAttack();  // 차지 공격
    public abstract void Skill1();          // 1스킬
    public abstract void Skill2();          // 2스킬
    public abstract void UltimateSkill();   // 궁극기
}
