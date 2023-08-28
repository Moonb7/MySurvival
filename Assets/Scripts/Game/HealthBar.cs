using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public CharacterStats characterStats;

    public Transform enemyHpbar; // �۽��� ĵ���� ��ü�� �����ϴ� ������Ʈ
    public Image healthImage;
   
    [Tooltip("hp�� Ǯ�̸� hpBar�� ������ ������ ���Ѵ�")]
    [SerializeField]
    private bool hideFullHealthBar = true;

    [Tooltip("HealthBar�� �Ĵٺ��� ���� ���Ѵ�.")]
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
            enemyHpbar.gameObject.SetActive(healthImage.fillAmount != 1); // 1�̸� false �ƴϸ� ture
        }

        CanvasGroup canvasGroup = enemyHpbar.GetComponent<CanvasGroup>();
        if (characterStats.isDeath)
        {
            if(canvasGroup != null && canvasGroup.alpha >= 0)
            {
                float countDonw =+ Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, countDonw/1); // �ｺ�ٰ� ���� ������� ���� ȿ��
                if(canvasGroup.alpha >= 0.01f && canvasGroup.alpha <= 0.1f)
                {
                    canvasGroup.alpha = 0;
                }
            }
        }
    }
}
