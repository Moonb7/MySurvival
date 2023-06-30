using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    // ���� ��ġ
    public Transform defaultWeaponPosition; // ��� ������ġ
    public Transform weaponmountPos;             // ������ġ

    // ���۹��� ���� - Player�� ���� ���⸦ 3���� ���ߴ�
    public List<WeaponController> startingWeapons = new List<WeaponController>();
    public WeaponController[] weaponSlots = new WeaponController[3];

    public static WeaponController activeWeapon;

    private float layerAtkModeTime;
    private float layerSwapTime;
    private float countDown;

    [Tooltip("���ݸ�� ���� �ð�")]
    [SerializeField]
    private float attackTime = 5f;
    private bool attackMode;
    //public static bool isAttack; // ���� �ִϸ��̼��� ������ Ÿ�̹��� �������� �������.

    // ���� ��ü
    private bool notChange = true;
    [SerializeField]
    private float changeweaponDelayTime = 1f;  // ���� ��ü ������ �ð�.
    [SerializeField]
    private float changeweaponEndDelayTime = 1f; // ���� ��ü ������ �ð�.

    private bool IsMounting
    {
        get { return PlayerNewInputController.animator.GetBool(AnimString.Instance.isMounting); }
        set { PlayerNewInputController.animator.SetBool(AnimString.Instance.isMounting, value); }
    }

    void Start()
    {
        // ���޹��� ���� ����
        foreach (var w in startingWeapons)
        {
            AddWeapon(w);
        }
        activeWeapon = weaponSlots[0];
    }

    void Update()
    {
        AttackMode();

        UpadateChange();
    }

    // ���� �ִϸ��̼� ���� �� ���ݻ��� ���� �� �������� ������ ������
    public void AttackMode()
    {
        PlayerNewInputController.animator.SetLayerWeight(1, layerAtkModeTime);
        PlayerNewInputController.animator.SetBool(AnimString.Instance.attackMode, attackMode);

        if (InputManager.Instance.attackKey && PlayerNewInputController.animator.GetBool(AnimString.Instance.isGround) && notChange)
        {
            if(activeWeapon != null)
            {
                StartCoroutine(AttackbyType()); // ���� ����

                layerAtkModeTime = 1;
                countDown = 0;
                attackMode = true;
            }
        }
        else if (countDown > attackTime && attackMode)
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

    //
    
    // Ÿ�Ժ� ����
    IEnumerator AttackbyType()
    {
        switch (activeWeapon.WeaponType)
        {
            case WeaponAttackType.Boxing:
                // �ִ� �׳� �ִϸ��̼����� ������ ���.
                break;
            case WeaponAttackType.Sword:  //----------------------------------------------------------
                Collider collider = activeWeapon.GetComponent<Collider>();

                yield return new WaitForSeconds(0.3f);
                collider.enabled = true;
                yield return new WaitForSeconds(0.2f);
                collider.enabled = false;

                break;
            case WeaponAttackType.Gun:

                break;
        }
        yield return null;
    }

    public float layerSpeed = 1.5f;
    void UpadateChange()
    {
        
        // ���� ��ȯ�� ��ü �ִ� ���̼Ǹ� ������ ����ũ�� ����� �̿��Ͽ����ϴ�.
        PlayerNewInputController.animator.SetLayerWeight(2, layerSwapTime);
        if (notChange)
        {
            if (InputManager.Instance.weapon1Key)
                StartCoroutine(WeaponInChange(0)); // 2������
            if (InputManager.Instance.weapon2Key)
                StartCoroutine(WeaponInChange(1)); // 2������
            if (InputManager.Instance.weapon3Key)
                StartCoroutine(WeaponInChange(2)); // 3�� ����

            layerSwapTime -= Time.deltaTime * layerSpeed;
            if (layerSwapTime <= 0)
            {
                layerSwapTime = 0;
            }
                
        }
    }

    // ���⸦ �ٲٰų� ������ �Լ�
    IEnumerator WeaponInChange(int ActiveWeaponIndex)
    {
        if (weaponSlots[ActiveWeaponIndex] == activeWeapon || PlayerNewInputController.animator.GetBool(AnimString.Instance.isAttack))
        {
            yield break; // ���� �ϸ� �ȵǴ� ���ǵ�
        }

        notChange = false;

        layerSwapTime = 1;
        

        if (IsMounting)
        {
            WeaponController oldWeapon = activeWeapon;

            IsMounting = false;
            yield return new WaitForSeconds(changeweaponDelayTime);

            // �����ڸ� ���� �ֱ�
            oldWeapon.transform.parent = defaultWeaponPosition;
            oldWeapon.transform.localPosition = Vector3.zero;          // transform�� ���� �� �θ� ������ ����
            oldWeapon.transform.localRotation = Quaternion.identity;


            // ���� ����
            if (weaponSlots[ActiveWeaponIndex] != null)
            {
                activeWeapon = weaponSlots[ActiveWeaponIndex];
                activeWeapon.transform.parent = weaponmountPos;
                activeWeapon.transform.localPosition = Vector3.zero;          // transform�� ���� �� �θ� ������ ����
                activeWeapon.transform.localRotation = Quaternion.identity;
            }
            
            IsMounting = true;
            PlayerNewInputController.animator.SetTrigger(AnimString.Instance.weaponChange);
            yield return new WaitForSeconds(changeweaponEndDelayTime);
        }
        else
        {
            if (weaponSlots[ActiveWeaponIndex] != null)
            {
                activeWeapon = weaponSlots[ActiveWeaponIndex];
                activeWeapon.transform.parent = weaponmountPos;
                activeWeapon.transform.localPosition = Vector3.zero;          // transform�� ���� �� �θ� ������ ����
                activeWeapon.transform.localRotation = Quaternion.identity;
            }

            IsMounting = true;
            PlayerNewInputController.animator.SetTrigger(AnimString.Instance.weaponChange);
            yield return new WaitForSeconds(changeweaponEndDelayTime);
        }

        ChangeSet(activeWeapon);
        
        notChange = true;
    }

    /*// ó�� �⺻ ����� ���ƿ��� �Լ�
    IEnumerator WeaponOutChange()
    {
        if (IsMounting == false)
            yield break;

        notChange = false;
        PlayerNewInputController.animator.SetLayerWeight(2, 1f);

        IsMounting = false;
        yield return new WaitForSeconds(changeweaponDelayTime);

        WeaponController oldWeapon = activeWeapon;
        oldWeapon.transform.parent = defaultWeaponPosition;
        oldWeapon.transform.localPosition = Vector3.zero;          // transform�� ���� �� �θ� ������ ����
        oldWeapon.transform.localRotation = Quaternion.identity;

        activeWeapon = weaponSlots[0];

        ChangeSet(activeWeapon);

        PlayerNewInputController.animator.SetLayerWeight(2, 0f);
        notChange = true;
    }*/

    void ChangeSet(WeaponController changeWeapon) // WeaponAttackType������ attackStats �� �°� �����ߴ�
    {
        if (changeWeapon == null) 
            return;
        PlayerNewInputController.animator.SetInteger(AnimString.Instance.weaponStats, (int)changeWeapon.WeaponType);

    }

    public bool AddWeapon(WeaponController weaponPrefab)
    {
        if(HasWeapon(weaponPrefab) != null)
        {
            Debug.Log("���� ���� ����");
            return false;
        }

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                // ���� ���� ����
                WeaponController weaponInstance = Instantiate(weaponPrefab, defaultWeaponPosition);
                weaponInstance.transform.localPosition = Vector3.zero;          // transform�� ���� �� �θ� ������ ����
                weaponInstance.transform.localRotation = Quaternion.identity;

                weaponInstance.Owner = gameObject; // �÷��̾��� ����� ����
                weaponInstance.weapon.SourcePrefab = weaponPrefab.gameObject;
                weaponInstance.gameObject.SetActive(true); // �̻� �ϸ� false �ϱ�

                weaponSlots[i] = weaponInstance;

                return true;
            }
        }

        Debug.Log("3�� �̻��̴�");
        return false;
    }

    // ���� ����
    public bool RemoveWeapon(WeaponController weaponInstance)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == weaponInstance)
            {
                weaponSlots[i] = null;

                Destroy(weaponInstance.gameObject);

                int windex = i <= 0 ? i + 1 : i - 1;
                StartCoroutine(WeaponInChange(windex)); // �ϴ� �̷��� �ϰ� ���߿� �ٲܹ��⸦ �ٷ� �ٲپ� �ִ� �������� ����
                
                return true;
            }
        }
        return false;
    }

    // �Ű������� ���� ���������� ������ ���Ⱑ ������ ������ ���⸦ ��ȯ ������ ���� ��ȯ
    public WeaponController HasWeapon(WeaponController weaponPrefab)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            var w = weaponSlots[i];
            if(w != null && w.weapon.SourcePrefab == weaponPrefab.gameObject)
            {
                return w;
            }
        }
        return null;
    }
}
