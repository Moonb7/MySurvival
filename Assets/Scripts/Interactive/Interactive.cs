using TMPro;
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

    /*public void Start()
    {
        InputManager.Instance.actions += Action; //�׼�Ű�� �̿��ؼ� ����Ͽ���.
    }*/

    public virtual void Update()
    {
        theDistance = Vector3.Distance(transform.position,player.position);
        {
            if (unIntreractive)
                return;

            //�Ÿ� ����ؼ� UI ���̰� ó��
            if (theDistance <= minDistance) // �̰� �ణ �׷��� ���� �׳� ���� ��ü�� ������ ����� ���������� "Player�� ������ �߰� ����
            {
                ShowActionUI();
            }
            else
            {
                HiddenActionUI();
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

    void Action()
    {
        if (theDistance <= minDistance)
        { 
            // �׼�
            DoAction();
        }
    }

    public virtual void DoAction()
    {
        HiddenActionUI();
    }
}
