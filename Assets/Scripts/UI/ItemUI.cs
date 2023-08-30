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

        // �÷��̾� ��Ʈ�ѷ� ó�� ���鵵 ���߾�� �ϴ� �����
        playerInput.enabled = false;
        enemyManager.PauseEnemies();

        //���콺 Ŀ��
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
