using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static UnityEngine.ParticleSystem;

public class InputManager : Singleton<InputManager>
{
    public Vector2 move;
    public Vector2 look;
    public float scrollWheel;

    [HideInInspector]
    public bool jumpKey;
    [HideInInspector]
    public bool sprintKey;
    [HideInInspector]
    public bool rollKey;
    [HideInInspector]
    public bool isPause = false;

    // ���� ���� ���� ���� �Ұ��ΰ�
    public bool analogMovement;
    public bool cursorLocked = true;        // Ŀ�����¸� ����°� 
    public bool cursorInputForLook = true;  // ���콺�� ���� �ٰ��ΰ�
    
    public static float chargingEnergy = 0f;       // ��¡�������� ���� ����
    private bool isCharging;                // ��¡ ������ üũ
    private Coroutine chargingcoroutine;
    private GameObject chagingEff;
    private GameObject chargingFullEff;      

    public Transform weaponEquipPos;

    WeaponManager weaponManager;

    private void Start()
    {
        SetCursorState(cursorLocked);
        weaponManager = GetComponent<WeaponManager>();
    }

    public void OnMove(InputAction.CallbackContext context) // ������ ��
    {
        if (isPause || GameManager.notSpawn)
            return;
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context) // ���콺 ��
    {
        if (cursorInputForLook)
        {
            look = context.ReadValue<Vector2>();
        }
    }

    public void OnJump(InputAction.CallbackContext context) // ���� Ű
    {
        if (isPause || GameManager.notSpawn)
            return;

        if (context.started)
        {
            jumpKey = true;
        }
        else if (context.canceled)
        {
            jumpKey = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext context) // �޸��� Ű
    {
        if (isPause)
            return;

        if (context.performed)
        {
            sprintKey = true;
            PlayerController.animator.SetBool(AnimString.instance.sprint, true); // ���� üũ
        }
        else if (context.canceled)
        {
            sprintKey = false;
            PlayerController.animator.SetBool(AnimString.instance.sprint, false);
        }
    }

    public void OnRoll(InputAction.CallbackContext context) // ������Ű
    {
        if (isPause || GameManager.notSpawn)
            return;

        if (context.performed)
        {
            if (context.interaction is PressInteraction)
            {
                rollKey = true;
            }
        }
        else if (context.canceled)
        {
            rollKey = false;
        }
    }
    
    public void OnScrollWheel(InputAction.CallbackContext context) // ���콺 ��ũ�Ѱ�
    {
        if (isPause || GameManager.notSpawn)
            return;

        scrollWheel = context.ReadValue<float>();
    }

    public void OnPause(InputAction.CallbackContext context) // �Ͻ����� 
    {
        if (context.started)
        {
            FindAnyObjectByType<PauseUI>().togle();
            isPause = !isPause;
        }
    }

    public void OnAttack(InputAction.CallbackContext context) // �⺻ ����
    {
        if (isPause || GameManager.notSpawn)
            return;

        if (WeaponManager.activeWeapon != null && WeaponManager.isWeaponSwichReady &&
            PlayerController.animator.GetBool(AnimString.Instance.isAttack) == false &&
            PlayerController.animator.GetBool(AnimString.Instance.isGround) && jumpKey == false && rollKey == false) // ���� �ִ���üũ �ٸ� �ൿ�� ���ϰ� �ִ���
        {
            if (context.performed) // Ű�� ������ �ִ� ����
            {
                isCharging = true;
                chagingEff = Instantiate(WeaponManager.activeWeapon.chargingEffect, this.transform);
                chagingEff.transform.localPosition = Vector3.zero;
                chagingEff.transform.localRotation = Quaternion.identity;

                if (chargingcoroutine != null)
                    StopCoroutine(chargingcoroutine);
                chargingcoroutine = StartCoroutine(Charging());
            }
            else if (chargingEnergy >= WeaponManager.activeWeapon.weaponScriptable.chargingEnergyTime) // ��¡ �������� �ٸ����� Ű�� ������ ��
            {
                WeaponManager.activeWeapon.ChargingAttack();
                isCharging = false;
                chargingEnergy = 0;
                WeaponManager.activeWeapon.weaponAudioSource.loop = false;
                Destroy(chagingEff);
                Destroy(chargingFullEff);
            }
            else if (context.canceled)
            {
                isCharging = false;
                chargingEnergy = 0;
                Debug.Log("���");
                WeaponManager.activeWeapon.weaponAudioSource.loop = false;

                if(chagingEff!= null)
                {
                    ParticleSystem ps = chagingEff.GetComponentInChildren<ParticleSystem>(); // �� �ȵ� ���

                    var mainModule = ps.main; // ��ƼŬ �ý����� main ����� ������
                    mainModule.loop = false;  // loop �Ӽ� ����
                }

                Destroy(chagingEff, 3f);
            }

            if (context.interaction is PressInteraction) // �Ϲ� ����
            {
                WeaponManager.activeWeapon.Attack();
            }
        }
    }

    IEnumerator Charging()
    {
        WeaponManager.activeWeapon.weaponAudioSource.clip = WeaponManager.activeWeapon.chargingSound;
        WeaponManager.activeWeapon.weaponAudioSource.loop= true;
        WeaponManager.activeWeapon.weaponAudioSource.Play();

        while (isCharging)
        {
            chargingEnergy += Time.deltaTime;
            Debug.Log("���");
            yield return null;
            if (chargingEnergy >= WeaponManager.activeWeapon.weaponScriptable.chargingEnergyTime)
            {
                // ��¡�������� �ٸ𿴴ٴ� ȿ�� ����Ʈ�� ��ȭ���� ���ٴ��� �׷���
                chargingFullEff = Instantiate(WeaponManager.activeWeapon.chagingFullEff, weaponManager.chagingFullEffPos);
                chargingFullEff.transform.localPosition = Vector3.zero;
                chargingFullEff.transform.localRotation = Quaternion.identity;
                WeaponManager.activeWeapon.weaponAudioSource.loop = false;
                yield break;
            }
        }
    }

    public void OnTargetting(InputAction.CallbackContext context) // ������ Ÿ�� ���� �ڼ��Ѱ� PlayerTargetting ����
    {
        if (isPause)
            return;
        if (WeaponManager.activeWeapon.weaponScriptable.weaponType == WeaponType.rangedweapon) // ���Ÿ� ����� Ÿ���� ���� �ʱ�
            return;

        if (context.started)
        {
            PlayerTargeting.targetEnemy = PlayerTargeting.enemy; // Ű�� ���� ������ ������ ���� ����
            PlayerTargeting.fastentargeting = !PlayerTargeting.fastentargeting;
        }
    }
    public void OnWeapon1(InputAction.CallbackContext context)
    {
        if (isPause)
            return;

        if (context.performed)
        {
            StartCoroutine(weaponManager.SwichWeapon(weaponManager.weaponSlots[0]));
        }
    }
    public void OnWeapon2(InputAction.CallbackContext context)
    {
        if (isPause)
            return;

        if (context.performed)
        {
            StartCoroutine(weaponManager.SwichWeapon(weaponManager.weaponSlots[1]));
        }
    }
    public void OnSkill1(InputAction.CallbackContext context)
    {
        if (isPause)
            return;

        if (WeaponManager.activeWeapon != null && WeaponManager.isWeaponSwichReady &&
            PlayerController.animator.GetBool(AnimString.Instance.isAttack) == false &&
            PlayerController.animator.GetBool(AnimString.Instance.isGround) && jumpKey == false && rollKey == false &&
            PlayerController.animator.GetBool(AnimString.Instance.canMove))
        {
            if (context.performed)
            {
                if (WeaponManager.activeWeapon.isSkill1Ready && WeaponManager.activeWeapon.comboCount == 0) // �Ϲݰ��ݰ� ��ġ�� ������ �־ �̷������� �غ��Ҵ�.
                {
                    WeaponManager.activeWeapon.Skill1();
                    WeaponManager.activeWeapon.skill1CoolTimedown = 0;
                }
                else
                {
                    Debug.Log("��ų1 ��Ÿ�� ���̴�.");
                }
            }
        }
    }

    public void OnSkill2(InputAction.CallbackContext context)
    {
        if (isPause)
            return;

        if (WeaponManager.activeWeapon != null && WeaponManager.isWeaponSwichReady &&
            PlayerController.animator.GetBool(AnimString.Instance.isAttack) == false &&
            PlayerController.animator.GetBool(AnimString.Instance.isGround) && jumpKey == false && rollKey == false &&
            PlayerController.animator.GetBool(AnimString.Instance.canMove))
        {
            if (context.performed)
            {
                if (WeaponManager.activeWeapon.isSkill2Ready && WeaponManager.activeWeapon.comboCount == 0)
                {
                    WeaponManager.activeWeapon.Skill2();
                    WeaponManager.activeWeapon.skill2CoolTimedown = 0;
                }
                else
                {
                    Debug.Log("��ų2 ��Ÿ�� ���̴�.");
                }
            }
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}

