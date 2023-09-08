using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemUI : MonoBehaviour
{
    public Transform thisUI; // 자기 자신 UI
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

        //EnemyManager.Instance.PauseEnemies();  // 적 움직임 막음

        //마우스 커서
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

        /*//마우스 커서
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
    }

    public virtual IEnumerator Close()
    {
        yield return new WaitForSeconds(0.1f);
        CloseUI();
    }
}
