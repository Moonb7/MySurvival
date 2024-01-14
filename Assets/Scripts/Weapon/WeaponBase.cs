using UnityEditor.Animations;
using UnityEngine;

// ���� ������Ʈ �����տ� ���� ������Ʈ Ŭ�����̴�. WeaponController �̴�.
public abstract class WeaponBase : MonoBehaviour
{
    [Tooltip("�ش� ������ �÷��̾� �ൿ �ִϸ��̼� ��Ʈ�ѷ�")]
    public AnimatorController aniController;
    [Tooltip("���� �⺻������ ����ִ� ���� ScriptableObject")]
    public WeaponScriptable weaponScriptable;
    [HideInInspector]
    public AudioSource weaponAudioSource;
    public Vector3 weaponPos; // ���� ������ġ
    public Vector3 weaponRot; // ���� ����
    protected WeaponManager weaponManager;
    protected CharacterStats characterStats;

    [Header("�� ������ ������ ���")]
    public float normalAttackDamageMultiplier = 1;
    public float chagingAttackDamageMultiplier = 1;
    public float skill1AttackDamageMultiplier = 1;
    public float skill2AttackDamageMultiplier = 1;

    public float DamageMultiplier { get; set; }
    public float StartDamageMultiplier { get; set; }
    public AttackState AttackState { get; set; }

    private void Start()
    {
        weaponAudioSource = GetComponent<AudioSource>();
        weaponManager = GetComponentInParent<WeaponManager>();
        characterStats = GetComponentInParent<CharacterStats>();
    }

    // ������ ���� �޺� ī����
    public int comboCount { get; set; }

    [Header("���� ���� ����Ʈ")]
    public GameObject attackEffect;         // �⺻���� ����Ʈ
    public GameObject chargingEffect;       // ��¡ �� ����Ʈ
    public GameObject chagingFullEff;       // ��¡ �տ� �ø��� ����Ʈ
    public GameObject chargingAttackEffect; // �������� ����Ʈ
    public GameObject skill1Effect;         // ��ų1 ����Ʈ
    public GameObject skill2Effect;         // ��ų2 ����Ʈ

    [Header("���� ���� ����")]
    public AudioClip attackSound;           // �⺻���� �Ҹ�
    public AudioClip chargingSound;         // ��¡ �� �Ҹ�
    public AudioClip chargingAttackSound;   // ��¡���� �Ҹ�
    public AudioClip skill1Sound;           // ��ų1 �Ҹ�
    public AudioClip skill2Sound;           // ��ų2 �Ҹ�

    [Header("������ų�� ������ ����ǥ�ÿ������Ʈ ����")]
    public GameObject buffImage;            // ����ǥ�ÿ������Ʈ

    public bool isSkill1Ready;              // ��ų1 ������ ��������
    public bool isSkill2Ready;              // ��ų2 ������ ��������
    public float skill1CoolTimedown = 100f;
    public float skill2CoolTimedown = 100f;


    protected void Update()
    {
        SkillCoolTimeCheck();
    }

    public abstract void Attack();          // �⺻ ����
    public abstract void ChargingAttack();  // ���� ����
    public abstract void Skill1();          // 1��ų
    public abstract void Skill2();          // 2��ų
    public abstract float AttackStatedamageMultiplier(); // ���ݻ��¿� ���� ������ �����ȯ

    public void SkillCoolTimeCheck() // ��Ÿ�� üũ
    {
        skill1CoolTimedown += Time.deltaTime;
        skill2CoolTimedown += Time.deltaTime;

        if (WeaponManager.activeWeapon)
        {
            isSkill1Ready = WeaponManager.activeWeapon.weaponScriptable.skill1Cool <= skill1CoolTimedown;
            isSkill2Ready = WeaponManager.activeWeapon.weaponScriptable.skill2Cool <= skill2CoolTimedown;
            if (isSkill1Ready)
            {
                skill1CoolTimedown = 100;
            }

            if (isSkill2Ready)
            {
                skill2CoolTimedown = 100;
            }
        }
    }

    public void AttackSetStats(AttackState _attackState) // �� �Լ��� ���� ������ �����ϰ� �׳� �Ϲݰ������� ������ų�������� �к��Ͽ� ���� �ϰ� �������. �������� ������ �ִϸ��̼� �̺�Ʈ �Լ��� �����Ͽ���.
    {
        AttackState = _attackState;
        PlayerController.animator.SetBool(AnimString.Instance.isAttack, true);
        PlayerController.animator.SetInteger(AnimString.Instance.attackStats, (int)AttackState);
    }
}
public enum AttackState
{
    attack, chargingAttack, skill1, skill2
}