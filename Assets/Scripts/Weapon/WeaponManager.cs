using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<WeaponBase> startingWeapons = new List<WeaponBase>();
    public WeaponBase[] weaponSlots = new WeaponBase[2];    // 게임 기획에 맞게 2개만 들고 다닐 수 있게 해놓았다. 나중에 privet으로 만들기 잠시 확인용
    public Transform defaultWeaponPos;                                  // 무기 대기위치
    public Transform weaponmountPos;                                    // 무기 장착위치
    public static WeaponBase activeWeapon;                        // 들고 있는 무기

    [SerializeField]
    private float attackModeTime = 5f;  // 공격모드 지속시간
    private float attackDelay = 0;      //
    
    private bool attackMode;            // 공격모드 중인지

    public float layerAtkModeTime;      
    private float countDown;

    private bool isChangeReady = true;         // 무기 교체가능한지
    private bool isAttackReady;         // 공격 가능한지

    private void Start()
    {
        foreach (var w in startingWeapons) 
        {
            AddWeapon(w);
        }
        activeWeapon = weaponSlots[0];
    }

    private void Update()
    {
        AttackMode();
    }

    public void AttackMode()
    {
        PlayerNewInputController.animator.SetLayerWeight(1, layerAtkModeTime);
        PlayerNewInputController.animator.SetBool(AnimString.Instance.attackMode, attackMode);

        attackDelay += Time.deltaTime;
        isAttackReady = attackDelay > activeWeapon.weaponScriptable.rateSpeed;

        if (activeWeapon != null && InputManager.Instance.attackKey && PlayerNewInputController.animator.GetBool(AnimString.Instance.isGround) // 공격키를 누르고 땅에 있고 무기교체상태가 아니고 공격준비중인지
            && isChangeReady && isAttackReady)
        { 
            Attack();                   // 실제 공격

            layerAtkModeTime = 1;
            countDown = 0;
            attackDelay = 0;
            attackMode = true;
        }
        else if (countDown > attackModeTime && attackMode)
        {
            if (layerAtkModeTime > 0)
            {
                layerAtkModeTime -= Time.deltaTime;

                if (layerAtkModeTime <= 0)
                {
                    layerAtkModeTime = 0;

                    attackMode = false;
                }
            }
        }
        countDown += Time.deltaTime;
    }

    public bool AddWeapon(WeaponBase weapon)
    {
        if (HasWeapon(weapon) != null)   // 이미 무기가 있으면
        {
            Debug.Log("같은 무기 있음");
            return false;
        }

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                // 새로 무기 생성
                WeaponBase weaponInstance = Instantiate(weapon, defaultWeaponPos);
                weaponInstance.transform.localPosition = Vector3.zero;
                weaponInstance.transform.localRotation = Quaternion.identity;
                weaponInstance.gameObject.SetActive(true); 

                weaponSlots[i] = weaponInstance;

                return true;
            }
        }
        return false;
    }

    public WeaponBase HasWeapon(WeaponBase weapon)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            var w = weaponSlots[i];
            if (w != null && w == weapon)
            {
                return w;
            }
        }
        return null;
    }

    private void Attack() // 실질적 공격
    {
        PlayerNewInputController.animator.SetTrigger(AnimString.Instance.attack);
    }
}
