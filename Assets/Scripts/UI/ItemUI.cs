using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemUI : MonoBehaviour
{
    public Transform thisUI; // �ڱ� �ڽ� UI
    protected bool isOpen;

    public virtual void Toggle()
    {
        if(isOpen)
        {
            StartCoroutine(Close());
        }
        else
        {
            OpenUI();
        }
    }

    public virtual void OpenUI()
    {
        isOpen = true;
        thisUI.gameObject.SetActive(isOpen);
        /*Animator animator = thisUI.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsOpen", isOpen);
        }*/

        //EnemyManager.Instance.PauseEnemies();  // �� ������ ����

        //���콺 Ŀ��
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public virtual void CloseUI()
    {
        isOpen = false;
        thisUI.gameObject.SetActive(isOpen);
        /*Animator animator = thisUI.GetComponent<Animator>();
        if(animator != null)
        {
            animator.SetBool("IsOpen", isOpen);
        }*/

        //EnemyManager.Instance.ResumeEnemies();

        /*//���콺 Ŀ��
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
    }

    public virtual IEnumerator Close()
    {
        yield return new WaitForSeconds(0.1f);
        CloseUI();
    }
}
