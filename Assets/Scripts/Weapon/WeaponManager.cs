using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    private CharacterStats characterStats;                              // 스텟상태

    public List<WeaponBase> startingWeapons = new List<WeaponBase>();
    public WeaponBase[] weaponSlots = new WeaponBase[2];                // 게임 기획에 맞게 2개제한
    public Transform defaultWeaponPos;                                  // 무기 대기위치
    public Transform weaponEquipPos;                                    // 무기 장착위치
    public Transform chagingFullEffPos;                                 // 풀차징 했을때의 이펙트 위치
    public static WeaponBase activeWeapon;                              // 현재 장착 무기

    [SerializeField]
    private float attackModeTime = 5f;                                  // 공격모드 지속시간
    private bool attackMode;                                            // 공격모드 중인지
    private float layerAtkModeTime;                                     // 공격모드지속 시간
    [SerializeField]
    private float comboDuration = 0.5f;                                 // 콤보공격 유지시간
    private float comboTimeDown;
    private float countDown;

    public static UnityAction OnSwichWeapon;

    public static bool isWeaponSwichReady = true;                       // 무기 변경이 가능한지
    private bool isEquip = true;                                        // 무기 장착중인지 확인용
    
    public float weaponSwitchDelay = 1f;                                // 처음 무기변겅 딜레이시간 무기변경할 때의 시각적인 시점을 보이기위해 만들었다.
    public float weaponSwitchDelay2 = 1f;                               // 두번쨰 무기변겅 딜레이 시간

    private PlayerTargeting playerTargeting;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        playerTargeting = GetComponent<PlayerTargeting>();
    }

    private void Start()
    {
        foreach (var w in startingWeapons)
        {
            AddWeapon(w);
        }
        EquipWeapon(weaponSlots[0]); // 시작 무기 장착
    }

    private void Update()
    {
        AttackMode();
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
        else if (comboTimeDown > comboDuration)
        {
            if(activeWeapon)
            activeWeapon.comboCount = 0; // 다시 처음 콤보 공격이 나가게 콤보카운트 초기화
        }
        if (activeWeapon)
            PlayerController.animator.SetInteger(AnimString.Instance.attackCombo, activeWeapon.comboCount);
    }

    public IEnumerator SwichWeapon(WeaponBase weapon)
    {
        if(activeWeapon == weapon)
        {
            yield break;
        }

        if (isWeaponSwichReady)
        {
            isWeaponSwichReady = false;

            PlayerController.animator.SetLayerWeight(2, 1);
            isEquip = false;
            PlayerController.animator.SetBool(AnimString.Instance.isEquip, isEquip);
            yield return new WaitForSecondsRealtime(weaponSwitchDelay); // 바꾸는 시간
            UnequipWeapon(activeWeapon);
            EquipWeapon(weapon);
            isEquip = true;
            PlayerController.animator.SetBool(AnimString.Instance.isEquip, isEquip);
            yield return new WaitForSecondsRealtime(weaponSwitchDelay2); // 바꾸는 시간
            PlayerController.animator.SetLayerWeight(2, 0);

            isWeaponSwichReady = true;
        }
    }

    private void EquipWeapon(WeaponBase weapon) // 가지고 있는 무기 장착
    {
        if (HasWeapon(weapon) == null) // 무기가 없으면
            return;

        activeWeapon = weapon;
        activeWeapon.transform.parent = weaponEquipPos;
        activeWeapon.transform.localPosition = weapon.weaponPos;
        activeWeapon.transform.localRotation = Quaternion.Euler(weapon.weaponRot);
        if(weapon.weaponScriptable.weaponType == WeaponType.rangedweapon)
        {
            playerTargeting.targetImage.gameObject.SetActive(true);
        }
        else if(weapon.weaponScriptable.weaponType == WeaponType.meleeweapon)
        {
            playerTargeting.targetImage.gameObject.SetActive(false);
        }

        PlayerController.animator.SetInteger(AnimString.Instance.weaponNum, weapon.weaponScriptable.weaponNum);
        OnSwichWeapon?.Invoke();
    }

    private void UnequipWeapon(WeaponBase weapon) // 가지고있는 무기 장착 해제
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
                    EquipWeapon(weaponSlots[windex]); // 나머지 무기 장착
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
}
