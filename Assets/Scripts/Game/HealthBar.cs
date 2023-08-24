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
    }
}
