using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<WeaponBase> startingWeapons = new List<WeaponBase>();
    private WeaponBase[] weaponSlots = new WeaponBase[2];               // 게임 기획에 맞게 2개제한
    public Transform defaultWeaponPos;                                  // 무기 대기위치
    public Transform weaponEquipPos;                                    // 무기 장착위치
    public static WeaponBase activeWeapon;                              // 현재 장착 무기

    [SerializeField]
    private float attackModeTime = 5f;                                  // 공격모드 지속시간
    private bool attackMode;                                            // 공격모드 중인지
    private float layerAtkModeTime;                                     // 공격모드지속 시간
    [SerializeField]
    private float comboOutTime = 0.5f;                                  // 콤보공격 유지시간
    private float countDown;                                            
    float comboTimeDown;

    public static bool isSkill1Ready;                                   // 스킬1 공격이 가능한지
    public static bool isSkill2Ready;                                   // 스킬2 공격이 가능한지
    public static float skill1CoolTimedown;                             
    public static float skill2CoolTimedown;                             

    public static bool isChangeReady = true;                            // 무기 교체가능한지

    private CharacterStats characterStats;                              // 스텟상태

    private void Awake()
    {
        foreach (var w in startingWeapons)
        {
            AddWeapon(w);
        }

        characterStats = GetComponent<CharacterStats>();
        if (weaponSlots[0] != null) // 시작 무기 장착
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

    private void AttackMode() // 공격 모드 유지및 공격조건들 초기화용도 이기도 하다.
    {
        if(characterStats.isDeath == true)
        {
            PlayerController.animator.SetLayerWeight(1,0);
            return;
        }

        PlayerController.animator.SetLayerWeight(1, layerAtkModeTime);
        PlayerController.animator.SetBool(AnimString.Instance.attackMode, attackMode);
        attackMode = layerAtkModeTime > 0;
        if (PlayerController.animator.GetBool(AnimString.Instance.isAttack))   // 공격 했는지 체크
        {
            layerAtkModeTime = 1;
            countDown = 0;
        }
        else if (countDown > attackModeTime && attackMode && layerAtkModeTime > 0) // 공격모드지속 시간이 지났고 공격 모드 상태인지
        {
            layerAtkModeTime -= Time.deltaTime;
        }
        countDown += Time.deltaTime;

        // 일정 시간동안 공격이 안들어 올시 콤보 초기화
        comboTimeDown += Time.deltaTime; // 흘러가는 시간
        if (PlayerController.animator.GetBool(AnimString.Instance.isAttack)) // 공격실행 하면
        {
            comboTimeDown = 0f;
        }
        else if (comboTimeDown > comboOutTime)
        {
            activeWeapon.comboCount = 0; // 다시 처음 콤보 공격이 나가게 콤보카운트 초기화
        }
        PlayerController.animator.SetInteger(AnimString.Instance.attackCombo, activeWeapon.comboCount);
    }

    private void WeaponEquip(WeaponBase weapon) // 가지고 있는 무기 장착
    {
        if (HasWeapon(weapon) == null) // 무기가 없으면
            return;

        activeWeapon = weapon;
        activeWeapon.transform.parent = weaponEquipPos;
        activeWeapon.transform.localPosition = Vector3.zero;
        activeWeapon.transform.localRotation = Quaternion.identity;
    }

    private void WeaponUnequip(WeaponBase weapon) // 가지고있는 무기 장착 해제
    {
        if (HasWeapon(weapon) == null) // 무기가 없으면
            return;

        activeWeapon = weapon;
        activeWeapon.transform.parent = defaultWeaponPos;
        activeWeapon.transform.localPosition = Vector3.zero;
        activeWeapon.transform.localRotation = Quaternion.identity;
    }

    public bool AddWeapon(WeaponBase weapon) // 무기 추가
    {
        if (HasWeapon(weapon) != null) // 이미 무기가 있으면
        {
            Debug.Log("같은 무기 있음");
            return false;
        }

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null) // 이미 무기가 있으면
            {
                WeaponBase weaponInstance = Instantiate(weapon, defaultWeaponPos); // 새로 무기 생성
                weaponInstance.transform.localPosition = Vector3.zero;
                weaponInstance.transform.localRotation = Quaternion.identity;
                weaponInstance.gameObject.SetActive(true); 

                weaponSlots[i] = weaponInstance;

                return true;
            }
        }
        return false;
    }

    // 무기 삭제
    public bool RemoveWeapon(WeaponBase weaponInstance)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == weaponInstance)
            {
                weaponSlots[i] = null;      // 슬록에서 null값으로 지정
                Destroy(weaponInstance);    // 아에 없애기

                int windex = i <= 0 ? i + 1 : i - 1;
                if(HasWeapon(weaponSlots[windex]) != null) // null이 아니면
                {
                    WeaponEquip(weaponSlots[windex]); // 나머지 무기 장착
                }
                return true;
            }
        }
        return false;
    }

    public WeaponBase HasWeapon(WeaponBase weapon) // 무기를 갖고 있는지 확인용
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

    private void SkillCoolTimeCheck() // 쿨타임 체크
    {
        skill1CoolTimedown += Time.deltaTime;
        skill2CoolTimedown += Time.deltaTime;

        isSkill1Ready = activeWeapon.weaponScriptable.skill1Cool <= skill1CoolTimedown;
        isSkill2Ready = activeWeapon.weaponScriptable.skill2Cool <= skill2CoolTimedown;
    }
}
