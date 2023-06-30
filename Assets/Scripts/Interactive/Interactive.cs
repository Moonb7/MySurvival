using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    private float theDistance;
    public Transform player;

    [SerializeField]
    private float minDistance = 2.0f;
    public string action = "Do Action";

    //Action UI
    public GameObject actionUI;
    public TextMeshProUGUI actionText;

    [Tooltip("üũ�� �⺻���� ��ȣ�ۿ��� �ȵǰ� ��������")]
    public bool unIntreractive = false;

    private void Update()
    {
        theDistance = Vector3.Distance(transform.position,player.position);
        {
            if (unIntreractive)
                return;

            //�Ÿ� ����ؼ� UI ���̰� ó��
            if (theDistance <= minDistance)
            {
                ShowActionUI();
            }
            else
            {
                HiddenActionUI();
            }

            //Action ó��
            if (InputManager.Instance.actionKey && theDistance <= minDistance)
            {
                HiddenActionUI();

                // �׼�
                DoAction();
            }
        }
    }

    public virtual void ShowActionUI()
    {
        actionUI.SetActive(true);

        actionText.text = action;
    }

    void HiddenActionUI()
    {
        actionUI.SetActive(false);
        actionText.text = "";
    }

    public virtual void DoAction()
    {
        // �ش� ������Ʈ�� ���ֵ� �ǰ� �������� ���ϴ� �׼� ���ϱ�
    }
}
