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

    [Tooltip("체크시 기본적인 상호작용이 안되게 설정가능")]
    public bool unIntreractive = false;

    private void Update()
    {
        theDistance = Vector3.Distance(transform.position,player.position);
        {
            if (unIntreractive)
                return;

            //거리 계산해서 UI 보이게 처리
            if (theDistance <= minDistance)
            {
                ShowActionUI();
            }
            else
            {
                HiddenActionUI();
            }

            //Action 처리
            if (InputManager.Instance.actionKey && theDistance <= minDistance)
            {
                HiddenActionUI();

                // 액션
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
        // 해당 오브젝트를 없애도 되고 여러가지 원하는 액션 취하기
    }
}
