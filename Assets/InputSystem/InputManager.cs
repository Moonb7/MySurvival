using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

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
    public bool weapon1Key;
    [HideInInspector]
    public bool weapon2Key;
    [HideInInspector]
    public bool weapon3Key;

    // 직접 구한 값을 통해 할것인가
    public bool analogMovement;

    public bool cursorLocked = true;        // 커서상태를 물어보는것 
    public bool cursorInputForLook = true;  // 마우스의 값을 줄것인가
    
    public float chargingEnergy = 0f;       // 차징에너지를 모을 변수
    private bool isCharging;                // 차징 중인지 체크
    private Coroutine chargingcoroutine;
    private GameObject chagingEff;
    private GameObject chargingFullEff;      

    public Transform weaponEquipPos;

    private void Start()
    {
        SetCursorState(cursorLocked);
    }

    public void OnMove(InputAction.CallbackContext context) // 움직임 값
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context) // 마우스 값
    {
        if (cursorInputForLook)
        {
            look = context.ReadValue<Vector2>();
        }
    }

    public void OnJump(InputAction.CallbackContext context) // 점프 키
    {
        if (context.started)
        {
            jumpKey = true;
        }
        else if (context.canceled)
        {
            jumpKey = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext context) // 달리기 키
    {
        if (context.started)
        {
            sprintKey = true;
            PlayerController.animator.SetBool(AnimString.instance.sprint, true); // 상태 체크
        }
        else if (context.canceled)
        {
            sprintKey = false;
            PlayerController.animator.SetBool(AnimString.instance.sprint, false);
        }
    }

    public void OnRoll(InputAction.CallbackContext context) // 구르기키
    {
        if (context.started)
        {
            rollKey = true;
        }
        else if (context.canceled)
        {
            rollKey = false;
        }
    }
    
    public void OnScrollWheel(InputAction.CallbackContext context) // 마우스 스크롤값
    {
        scrollWheel = context.ReadValue<float>();
    }

    public void OnPause(InputAction.CallbackContext context) // 일시정지 
    {
        if (context.started)
        {
            FindAnyObjectByType<PauseUI>().togle();
        }
    }

    public void OnAttack(InputAction.CallbackContext context) // 기본 공격
    {
        if (WeaponManager.activeWeapon != null && WeaponManager.isChangeReady &&
            PlayerController.animator.GetBool(AnimString.Instance.isAttack) == false &&
            PlayerController.animator.GetBool(AnimString.Instance.isGround) && jumpKey == false && rollKey == false ) // 땅에 있는지체크 다른 행동을 취하고 있는지
        {
            if (context.performed) // 키를 누르고 있는 동안
            {
                isCharging = true;
                chagingEff = Instantiate(WeaponManager.activeWeapon.chargingEffect, this.transform);
                chagingEff.transform.localPosition = Vector3.zero;
                chagingEff.transform.localRotation = Quaternion.identity;

                if (chargingcoroutine != null)
                    StopCoroutine(chargingcoroutine);
                chargingcoroutine = StartCoroutine(charging());
            }
            else if (chargingEnergy >= WeaponManager.activeWeapon.weaponScriptable.chargingEnergyTime) // 다모으고 키를 떼었을 때
            {
                WeaponManager.activeWeapon.ChargingAttack();
                isCharging = false;
                chargingEnergy = 0;
                Debug.Log("공격");
                Destroy(chagingEff, 1f);
                Destroy(chargingFullEff, 0.3f);
            }
            else if (context.canceled)
            {
                isCharging = false;
                chargingEnergy = 0;
                Debug.Log("취소");
                Destroy(chagingEff, 1f);
            }

            if (context.interaction is PressInteraction) // 일반 공격
            {
                WeaponManager.activeWeapon.Attack();
            }
        }
    }

    IEnumerator charging()
    {
        while (isCharging)
        {
            chargingEnergy += Time.deltaTime;
            Debug.Log("헐딩");
            yield return null;
            if (chargingEnergy >= WeaponManager.activeWeapon.weaponScriptable.chargingEnergyTime)
            {
                // 차징에너지가 다모였다는 효과 이펙트가 더화려해 진다던다 그런거
                chargingFullEff = Instantiate(WeaponManager.activeWeapon.chagingFullEff, weaponEquipPos);
                chargingFullEff.transform.localPosition = Vector3.zero;
                chargingFullEff.transform.localRotation = Quaternion.identity;
                yield break;
            }
        }
    }

    public void OnTargetting(InputAction.CallbackContext context) // 고정할 타켓 설정 자세한건 PlayerTargetting 참고
    {
        if (context.started)
        {
            PlayerTargeting.targetEnemy = PlayerTargeting.enemy; // 키를 누를 때마다 고정할 적을 갱신
            PlayerTargeting.fastentargeting = !PlayerTargeting.fastentargeting;
        }
    }
    public void Weapon1(InputAction.CallbackContext context)
    {
        if (context.started && !weapon1Key)
        {
            weapon1Key = true;
        }
        else if (context.canceled)
        {
            weapon1Key = false;
        }
    }
    public void Weapon2(InputAction.CallbackContext context)
    {
        if (context.started && !weapon2Key)
        {
            weapon2Key = true;
        }
        else if (context.canceled)
        {
            weapon2Key = false;
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

