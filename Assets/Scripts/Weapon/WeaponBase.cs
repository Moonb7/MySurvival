using UnityEditor.Animations;
using UnityEngine;

// ���� ������Ʈ �����տ� ���� ������Ʈ Ŭ�����̴�. WeaponController �̴�.
public abstract class WeaponBase : MonoBehaviour
{
    [Tooltip("���� �⺻������ ����ִ� ���� ScriptableObject")]
    public WeaponScriptable weaponScriptable;
    [Tooltip("�ش� ������ �ִϸ�������Ʈ�ѷ�")] // �̷��� �ش� ������ �ִϸ����͸� ����� �÷��̾��� ��Ʈ�ѷ��� ��ü�� ���ָ� �����ϰ� �� �ش� ������ �ִϸ��̼��� ���� ���� ������ �� ����.
    public AnimatorController weaponAnimator;

    // ������ ���� �޺� ī����
    public int comboCount { get; set; }

    [Header("���� ���� ����Ʈ")]
    public GameObject attackEffect;         // �⺻���� ����Ʈ
    public GameObject dashAttackEffect;     // �뽬���� ����Ʈ
    public GameObject chargingEffect;       // ��¡ �� ����Ʈ
    public GameObject chargingAttackEffect; // �������� ����Ʈ
    public GameObject skill1Effect;         // ��ų1 ����Ʈ
    public GameObject skill2Effect;         // ��ų2 ����Ʈ
    public GameObject ultimateSkillEffect;  // �ñر� ����Ʈ

    [Header("���� ���� ����")]
    public AudioClip attackSound;           // �⺻���� �Ҹ�
    public AudioClip dashAttackSound;       // �뽬���� �Ҹ�
    public AudioClip chargingSound;         // ��¡ �� �Ҹ�
    public AudioClip chargingAttackSound;   // ��¡���� �Ҹ�
    public AudioClip skill1Sound;           // ��ų1 �Ҹ�
    public AudioClip skill2Sound;           // ��ų2 �Ҹ�
    public AudioClip ultimateSkillSound;    // �ñ��� �Ҹ�

    public abstract void Attack();          // �⺻ ����
    public abstract void DashAttack();      // ��� ����
    public abstract void ChargingAttack();  // ���� ����
    public abstract void Skill1();          // 1��ų
    public abstract void Skill2();          // 2��ų
    public abstract void UltimateSkill();   // �ñر�
}
