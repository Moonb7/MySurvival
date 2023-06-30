using UnityEngine;
using UnityEngine.InputSystem;

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
    public bool attackKey;
    [HideInInspector]
    public bool weapon1Key;
    [HideInInspector]
    public bool weapon2Key;
    [HideInInspector]
    public bool weapon3Key;
    [HideInInspector]
    public bool actionKey;

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
        if (context.started && !attackKey)
        {
            attackKey = true;
            PlayerNewInputController.animator.SetTrigger(AnimString.Instance.attack);
            // 여기에서 추가적인 공격 동작을 호출하거나 다른 동작을 수행할 수 있습니다.

        }
        else if (context.canceled)
        {
            attackKey = false;
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
    public void Weapon3(InputAction.CallbackContext context)
    {
        if (context.started && !weapon3Key)
        {
            weapon3Key = true;
        }
        else if (context.canceled)
        {
            weapon3Key = false;
        }
    }
    public void Action(InputAction.CallbackContext context)
    {
        if (context.started && !actionKey)
        {
            actionKey = true;
        }
        else if (context.canceled)
        {
            actionKey = false;
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

