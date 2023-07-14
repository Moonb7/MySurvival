using System.Collections;
using UnityEngine;
using UnityEngine.Events;
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

    public bool cursorLocked = true;       // 커서상태를 물어보는것 
    public bool cursorInputForLook = true; // 마우스의 값을 줄것인가

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
        }
        else if (context.canceled)
        {
            sprintKey = false;
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
        if (WeaponManager.activeWeapon != null && WeaponManager.isChangeReady && // 공격 준비상태인지
            PlayerNewInputController.animator.GetBool(AnimString.Instance.isGround) && jumpKey == false && rollKey == false )                            // 땅에 있는지체크
        {
            if (context.performed)
            {
                if (context.interaction is HoldInteraction)      // 차징 공격
                {
                    float chagingEnergy = +Time.deltaTime;
                    // 여기에 모으는 이벤트 액션이 들어가야된다.
                    Debug.Log($"차징 : {chagingEnergy}");
                    if (chagingEnergy > WeaponManager.activeWeapon.weaponScriptable.chargingEnergyTime /*&& */) // 모으는 시간을 넘기고 키를 때면
                    {
                        // 조건 완료 표시도 있어야 할거 같다. 이펙트활용을 하자
                        WeaponManager.activeWeapon.ChargingAttack();
                    }
                }
                else if (context.interaction is PressInteraction) // 일반 공격
                {
                    WeaponManager.activeWeapon.Attack();
                }
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

