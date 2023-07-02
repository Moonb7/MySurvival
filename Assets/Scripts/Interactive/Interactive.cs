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

    [Tooltip("체크시 기본적인 상호작용이 안되게 설정가능")]
    public bool unIntreractive = false;

    /*public void Start()
    {
        InputManager.Instance.actions += Action; //액션키를 이용해서 사용하였다.
    }*/

    public virtual void Update()
    {
        theDistance = Vector3.Distance(transform.position,player.position);
        {
            if (unIntreractive)
                return;

            //거리 계산해서 UI 보이게 처리
            if (theDistance <= minDistance) // 이게 약간 그런가 보네 그냥 여기 자체에 범위를 만들고 범위안으로 "Player가 들어오면 뜨게 하자
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
            // 액션
            DoAction();
        }
    }

    public virtual void DoAction()
    {
        HiddenActionUI();
    }
}
