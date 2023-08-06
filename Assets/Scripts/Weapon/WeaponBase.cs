using UnityEditor.Animations;
using UnityEngine;

// 무기 오브젝트 프리팹에 넣을 컴포넌트 클래스이다. WeaponController 이다.
public abstract class WeaponBase : MonoBehaviour
{
    [Tooltip("무기 기본정보가 들어있는 무기 ScriptableObject")]
    public WeaponScriptable weaponScriptable;
    public AudioSource weaponAudioSource;
    protected WeaponManager weaponManager;
    protected CharacterStats characterStats;
    
    public float damageMultiplier { get; set; }
    public float startDamageMultiplier { get; set; }
    public AttackState attackState { get; set; }

    private void Start()
    {
        weaponManager = GetComponentInParent<WeaponManager>();
        characterStats = GetComponentInParent<CharacterStats>();
    }

    // 무기의 현재 콤보 카운터
    public int comboCount { get; set; }

    [Header("무기 공격 이펙트")]
    public GameObject attackEffect;         // 기본공격 이펙트
    public GameObject chargingEffect;       // 차징 중 이펙트
    public GameObject chagingFullEff;       // 차징 손에 올리는 이펙트
    public GameObject chargingAttackEffect; // 차지공격 이펙트
    public GameObject skill1Effect;         // 스킬1 이펙트
    public GameObject skill2Effect;         // 스킬2 이펙트

    [Header("무기 공격 사운드")]
    public AudioClip attackSound;           // 기본공격 소리
    public AudioClip chargingSound;         // 차징 중 소리
    public AudioClip chargingAttackSound;   // 차징공격 소리
    public AudioClip skill1Sound;           // 스킬1 소리
    public AudioClip skill2Sound;           // 스킬2 소리

    [Header("버프스킬이 있을시 버프표시용오브젝트 적용")]
    public GameObject buffImage;            // 버프표시용오브젝트

    public abstract void Attack();          // 기본 공격
    public abstract void ChargingAttack();  // 차지 공격
    public abstract void Skill1();          // 1스킬
    public abstract void Skill2();          // 2스킬
    public abstract float AttackStatedamageMultiplier(); // 공격상태에 따른 데미지 계수변환

    public void AttackSetStats(AttackState _attackState)
    {
        attackState = _attackState;
        PlayerController.animator.SetBool(AnimString.Instance.isAttack, true);
        PlayerController.animator.SetInteger(AnimString.Instance.attackStats, (int)attackState);
    }
}
public enum AttackState
{
    attack, chargingAttack, skill1, skill2
}