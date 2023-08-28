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

    [Tooltip("HealthBar가 쳐다볼지 말지 정한다.")]
    [SerializeField]
    private bool lookHealthBar;

    private void Update()
    {
        healthImage.fillAmount = characterStats.CurrentHealth / characterStats.maxHealth.GetValue();

        if (lookHealthBar)
        {
            if (enemyHpbar != null)
                enemyHpbar.LookAt(Camera.main.transform);
        }
        if (hideFullHealthBar)
        {
            enemyHpbar.gameObject.SetActive(healthImage.fillAmount != 1); // 1이면 false 아니면 ture
        }

        CanvasGroup canvasGroup = enemyHpbar.GetComponent<CanvasGroup>();
        if (characterStats.isDeath)
        {
            if(canvasGroup != null && canvasGroup.alpha >= 0)
            {
                float countDonw =+ Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, countDonw/1); // 헬스바가 점점 사라지는 연출 효과
                if(canvasGroup.alpha >= 0.01f && canvasGroup.alpha <= 0.1f)
                {
                    canvasGroup.alpha = 0;
                }
            }
        }
    }
}
