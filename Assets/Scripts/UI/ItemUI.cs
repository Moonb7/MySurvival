using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemUI : MonoBehaviour
{
    public bool isOpen { get; private set; }

    private PlayerInput playerInput;
    private EnemyManager enemyManager;

    protected virtual void Start()
    {
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        enemyManager = FindAnyObjectByType<EnemyManager>();
    }

    public virtual void Toggle()
    {
        if(isOpen)
        {

        }
        else
        {

        }
    }

    public virtual void OpenUI()
    {
        isOpen = true;

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsOpen", isOpen);
        }

        // 플레이어 컨트롤러 처리 적들도 멈추어야 하니 고민좀
        playerInput.enabled = false;
        enemyManager.PauseEnemies();

        //마우스 커서
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
