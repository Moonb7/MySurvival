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

    // ���� ���� ���� ���� �Ұ��ΰ�
    public bool analogMovement;

    public bool cursorLocked = true;       // Ŀ�����¸� ����°� 
    public bool cursorInputForLook = true; // ���콺�� ���� �ٰ��ΰ�

    private void Start()
    {
        SetCursorState(cursorLocked);
    }

    public void OnMove(InputAction.CallbackContext context) // ������ ��
    {
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
        if (context.started)
        {
            sprintKey = true;
        }
        else if (context.canceled)
        {
            sprintKey = false;
        }
    }

    public void OnRoll(InputAction.CallbackContext context) // ������Ű
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
    
    public void OnScrollWheel(InputAction.CallbackContext context) // ���콺 ��ũ�Ѱ�
    {
        scrollWheel = context.ReadValue<float>();
    }

    public void OnPause(InputAction.CallbackContext context) // �Ͻ����� 
    {
        if (context.started)
        {
            FindAnyObjectByType<PauseUI>().togle();
        }
    }

    public void OnAttack(InputAction.CallbackContext context) // �⺻ ����
    {
        if (context.started && !attackKey)
        {
            attackKey = true;
            PlayerNewInputController.animator.SetTrigger(AnimString.Instance.attack);
            // ���⿡�� �߰����� ���� ������ ȣ���ϰų� �ٸ� ������ ������ �� �ֽ��ϴ�.

        }
        else if (context.canceled)
        {
            attackKey = false;
        }
    }

    public void OnTargetting(InputAction.CallbackContext context) // ������ Ÿ�� ���� �ڼ��Ѱ� PlayerTargetting ����
    {
        if (context.started)
        {
            PlayerTargeting.targetEnemy = PlayerTargeting.enemy; // Ű�� ���� ������ ������ ���� ����
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

