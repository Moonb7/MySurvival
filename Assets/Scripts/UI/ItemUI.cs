using UnityEngine;

public class ItemUI : MonoBehaviour
{
    public Transform thisUI; // �ڱ� �ڽ� UI
    protected bool isOpen;

    public virtual void Toggle()
    {
        if(isOpen)
        {
            CloseUI();
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

        //���콺 Ŀ��
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public virtual void CloseUI()
    {
        isOpen = false;
        thisUI.gameObject.SetActive(isOpen);
    }

    /*public virtual IEnumerator Close()
    {
        yield return new WaitForSeconds(0.1f);
        CloseUI();
    }*/
}
