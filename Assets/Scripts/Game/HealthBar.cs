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

    [Tooltip("HealthBar�� ī�޶� �Ĵٺ��� ���� ���Ѵ�.")]
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
            enemyHpbar.gameObject.SetActive(healthImage.fillAmount != 1); // 1�̸� false �ƴϸ� ture
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
            float fadeSpeed = 0.5f; // ���̵� �ӵ� ����
            float alpha = canvasGroup.alpha - fadeSpeed * Time.deltaTime;
            canvasGroup.alpha = Mathf.Max(alpha, 0); // ���İ��� ������ ���� �ʵ��� ó��
        }*/
        if (canvasGroup.alpha <= 0.1f)
        {
            canvasGroup.alpha = 0;
            enemyHpbar.gameObject.SetActive(false);
        }
    }
}
