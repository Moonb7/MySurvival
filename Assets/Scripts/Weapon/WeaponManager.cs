using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    private CharacterStats characterStats;                              // ���ݻ���

    public List<WeaponBase> startingWeapons = new List<WeaponBase>();
    public WeaponBase[] weaponSlots = new WeaponBase[2];                // ���� ��ȹ�� �°� 2������
    public Transform defaultWeaponPos;                                  // ���� �����ġ
    public Transform weaponEquipPos;                                    // ���� ������ġ
    public Transform chagingFullEffPos;                                 // Ǯ��¡ �������� ����Ʈ ��ġ
    public static WeaponBase activeWeapon;                              // ���� ���� ����

    [SerializeField]
    private float attackModeTime = 5f;                                  // ���ݸ�� ���ӽð�
    private bool attackMode;                                            // ���ݸ�� ������
    private float layerAtkModeTime;                                     // ���ݸ������ �ð�
    [SerializeField]
    private float comboDuration = 0.5f;                                 // �޺����� �����ð�
    private float comboTimeDown;
    private float countDown;

    public static UnityAction OnSwichWeapon;

    public static bool isWeaponSwichReady = true;                       // ���� ������ ��������
    private bool isEquip = true;                                        // ���� ���������� Ȯ�ο�
    
    public float weaponSwitchDelay = 1f;                                // ó�� ���⺯�� �����̽ð� ���⺯���� ���� �ð����� ������ ���̱����� �������.
    public float weaponSwitchDelay2 = 1f;                               // �ι��� ���⺯�� ������ �ð�

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
        EquipWeapon(weaponSlots[0]); // ���� ���� ����
    }

    private void Update()
    {
        AttackMode();
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
        else if (comboTimeDown > comboDuration)
        {
            if(activeWeapon)
            activeWeapon.comboCount = 0; // �ٽ� ó�� �޺� ������ ������ �޺�ī��Ʈ �ʱ�ȭ
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
            yield return new WaitForSecondsRealtime(weaponSwitchDelay); // �ٲٴ� �ð�
            UnequipWeapon(activeWeapon);
            EquipWeapon(weapon);
            isEquip = true;
            PlayerController.animator.SetBool(AnimString.Instance.isEquip, isEquip);
            yield return new WaitForSecondsRealtime(weaponSwitchDelay2); // �ٲٴ� �ð�
            PlayerController.animator.SetLayerWeight(2, 0);

            isWeaponSwichReady = true;
        }
    }

    private void EquipWeapon(WeaponBase weapon) // ������ �ִ� ���� ����
    {
        if (HasWeapon(weapon) == null) // ���Ⱑ ������
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

    private void UnequipWeapon(WeaponBase weapon) // �������ִ� ���� ���� ����
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
                    EquipWeapon(weaponSlots[windex]); // ������ ���� ����
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
}
