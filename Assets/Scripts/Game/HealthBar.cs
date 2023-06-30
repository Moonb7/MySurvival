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

    private void Update()
    {
        healthImage.fillAmount = characterStats.CurrentHealth / characterStats.maxHealth.GetValue();

        enemyHpbar.LookAt(Camera.main.transform);

        if (hideFullHealthBar)
        {
            enemyHpbar.gameObject.SetActive(healthImage.fillAmount != 1); // 1�̸� false �ƴϸ� ture
        }
    }
}
