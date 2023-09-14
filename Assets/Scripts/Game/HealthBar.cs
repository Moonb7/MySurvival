using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public CharacterStats characterStats;

    public Transform enemyHpbar; // 핼스바 캔버스 전체를 관리하는 오브젝트
    public Image healthImage;
   
    [Tooltip("hp가 풀이면 hpBar를 숨길지 말지를 정한다")]
    [SerializeField]
    private bool hideFullHealthBar = true;

    [Tooltip("HealthBar가 카메라를 쳐다볼지 말지 정한다.")]
    [SerializeField]
    private bool lookHealthBar;

    private CanvasGroup canvasGroup;
    private Animator UIanimator;

    private void Start()
    {
        canvasGroup = enemyHpbar.GetComponent<CanvasGroup>();
        UIanimator = enemyHpbar.GetComponent<Animator>();
    }

    private void Update()
    {
        healthImage.fillAmount = characterStats.CurrentHealth / characterStats.maxHealth.GetValue();

        if (lookHealthBar)
        {
            if (enemyHpbar != null)
                enemyHpbar.LookAt(Camera.main.transform);
                //enemyHpbar.LookAt(enemyHpbar.position + Camera.main.transform.rotation * Vector3.forward,Camera.main.transform.rotation * Vector3.up);
        }
        if (hideFullHealthBar)
        {
            enemyHpbar.gameObject.SetActive(healthImage.fillAmount != 1); // 1이면 false 아니면 ture
        }


        if (canvasGroup != null && UIanimator != null && characterStats.isDeath)
        {
            UIanimator.SetBool("IsDeath", true);
            FadeOutHealthBar();
        }
    }

    private void FadeOutHealthBar()
    {
        /*if (canvasGroup != null && canvasGroup.alpha > 0)
        {
            float fadeSpeed = 0.5f; // 페이드 속도 조절
            float alpha = canvasGroup.alpha - fadeSpeed * Time.deltaTime;
            canvasGroup.alpha = Mathf.Max(alpha, 0); // 알파값이 음수가 되지 않도록 처리
        }*/
        if (canvasGroup.alpha <= 0.1f)
        {
            canvasGroup.alpha = 0;
            enemyHpbar.gameObject.SetActive(false);
        }
    }
}
