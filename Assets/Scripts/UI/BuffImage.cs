using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffImage : MonoBehaviour
{
    private Image buffImage;
    private TextMeshProUGUI buffDownTimeText;
    private float buffcoolDown;
    private float buffTime;

    private void OnEnable()
    {
        buffImage = GetComponent<Image>();
        buffDownTimeText= GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        buffTime = WeaponManager.activeWeapon.weaponScriptable.buffTime;
    }

    private void Update()
    {
        buffcoolDown += Time.deltaTime;
        if (WeaponManager.activeWeapon!= null)
        {
            float bufftime = buffTime - buffcoolDown;
            buffDownTimeText.text = bufftime.ToString("0.0");
        }

        buffImage.fillAmount = 1 - (buffcoolDown / WeaponManager.activeWeapon.weaponScriptable.buffTime);

        if(buffImage.fillAmount <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
