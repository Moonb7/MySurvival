using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<WeaponBase> startingWeapons = new List<WeaponBase>();
    private WeaponBase[] weaponSlots = new WeaponBase[2];               // ���� ��ȹ�� �°� 2������
    public Transform defaultWeaponPos;                                  // ���� �����ġ
    public Transform weaponEquipPos;                                    // ���� ������ġ
    public static WeaponBase activeWeapon;                              // ���� ���� ����

    [SerializeField]
    private float attackModeTime = 5f;                                  // ���ݸ�� ���ӽð�
    private bool attackMode;                                            // ���ݸ�� ������
    private float layerAtkModeTime;                                     // ���ݸ������ �ð�
    [SerializeField]
    private float comboOutTime = 0.5f;                                  // �޺����� �����ð�
    private float countDown;                                            
    float comboTimeDown;

    public static bool isSkill1Ready;                                   // ��ų1 ������ ��������
    public static bool isSkill2Ready;                                   // ��ų2 ������ ��������
    public static float skill1CoolTimedown;                             
    public static float skill2CoolTimedown;                             

    public static bool isChangeReady = true;                            // ���� ��ü��������

    private CharacterStats characterStats;                              // ���ݻ���

    private void Awake()
    {
        foreach (var w in startingWeapons)
        {
            AddWeapon(w);
        }

        characterStats = GetComponent<CharacterStats>();
        if (weaponSlots[0] != null) // ���� ���� ����
        {
            activeWeapon = weaponSlots[0];
            activeWeapon.transform.parent = weaponEquipPos;
            activeWeapon.transform.localPosition = Vector3.zero;
            activeWeapon.transform.localRotation = Quaternion.identity;
            skill1CoolTimedown = activeWeapon.weaponScriptable.skill1Cool;
            skill2CoolTimedown = activeWeapon.weaponScriptable.skill2Cool;
        }
    }
    private void Start()
    {
        SkillUI.Instance.SetskillImage();
    }

    private void Update()
    {
        AttackMode();
        SkillCoolTimeCheck();
    }

    private void AttackMode() // ���� ��� ������ �������ǵ� �ʱ�ȭ�뵵 �̱⵵ �ϴ�.
    {
        if(characterStats.isDeath == true)
        {
            PlayerController.animator.SetLayerWeight(1,0);
            return;
        }

        PlayerController.animator.SetLayerWeight(1, layerAtkModeTime);
        PlayerController.animator.SetBool(AnimString.Instance.attackMode, attackMode);
        attackMode = layerAtkModeTime > 0;
        if (PlayerController.animator.GetBool(AnimString.Instance.isAttack))   // ���� �ߴ��� üũ
        {
            layerAtkModeTime = 1;
            countDown = 0;
        }
        else if (countDown > attackModeTime && attackMode && layerAtkModeTime > 0) // ���ݸ������ �ð��� ������ ���� ��� ��������
        {
            layerAtkModeTime -= Time.deltaTime;
        }
        countDown += Time.deltaTime;

        // ���� �ð����� ������ �ȵ�� �ý� �޺� �ʱ�ȭ
        comboTimeDown += Time.deltaTime; // �귯���� �ð�
        if (PlayerController.animator.GetBool(AnimString.Instance.isAttack)) // ���ݽ��� �ϸ�
        {
            comboTimeDown = 0f;
        }
        else if (comboTimeDown > comboOutTime)
        {
            activeWeapon.comboCount = 0; // �ٽ� ó�� �޺� ������ ������ �޺�ī��Ʈ �ʱ�ȭ
        }
        PlayerController.animator.SetInteger(AnimString.Instance.attackCombo, activeWeapon.comboCount);
    }

    private void WeaponEquip(WeaponBase weapon) // ������ �ִ� ���� ����
    {
        if (HasWeapon(weapon) == null) // ���Ⱑ ������
            return;

        activeWeapon = weapon;
        activeWeapon.transform.parent = weaponEquipPos;
        activeWeapon.transform.localPosition = Vector3.zero;
        activeWeapon.transform.localRotation = Quaternion.identity;
    }

    private void WeaponUnequip(WeaponBase weapon) // �������ִ� ���� ���� ����
    {
        if (HasWeapon(weapon) == null) // ���Ⱑ ������
            return;

        activeWeapon = weapon;
        activeWeapon.transform.parent = defaultWeaponPos;
        activeWeapon.transform.localPosition = Vector3.zero;
        activeWeapon.transform.localRotation = Quaternion.identity;
    }

    public bool AddWeapon(WeaponBase weapon) // ���� �߰�
    {
        if (HasWeapon(weapon) != null) // �̹� ���Ⱑ ������
        {
            Debug.Log("���� ���� ����");
            return false;
        }

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null) // �̹� ���Ⱑ ������
            {
                WeaponBase weaponInstance = Instantiate(weapon, defaultWeaponPos); // ���� ���� ����
                weaponInstance.transform.localPosition = Vector3.zero;
                weaponInstance.transform.localRotation = Quaternion.identity;
                weaponInstance.gameObject.SetActive(true); 

                weaponSlots[i] = weaponInstance;

                return true;
            }
        }
        return false;
    }

    // ���� ����
    public bool RemoveWeapon(WeaponBase weaponInstance)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == weaponInstance)
            {
                weaponSlots[i] = null;      // ���Ͽ��� null������ ����
                Destroy(weaponInstance);    // �ƿ� ���ֱ�

                int windex = i <= 0 ? i + 1 : i - 1;
                if(HasWeapon(weaponSlots[windex]) != null) // null�� �ƴϸ�
                {
                    WeaponEquip(weaponSlots[windex]); // ������ ���� ����
                }
                return true;
            }
        }
        return false;
    }

    public WeaponBase HasWeapon(WeaponBase weapon) // ���⸦ ���� �ִ��� Ȯ�ο�
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] != null && weaponSlots[i] == weapon)
            {
                return weaponSlots[i];
            }
        }
        return null;
    }

    private void SkillCoolTimeCheck() // ��Ÿ�� üũ
    {
        skill1CoolTimedown += Time.deltaTime;
        skill2CoolTimedown += Time.deltaTime;

        isSkill1Ready = activeWeapon.weaponScriptable.skill1Cool <= skill1CoolTimedown;
        isSkill2Ready = activeWeapon.weaponScriptable.skill2Cool <= skill2CoolTimedown;
    }
}
