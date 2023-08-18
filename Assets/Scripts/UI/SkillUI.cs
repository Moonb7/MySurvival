using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : Singleton<SkillUI>
{
    public Image weaponImage;               // ���� �����̹���
    public Image skillImage1;               // ��ų1 �̹���
    public Image skillImage2;               // ��ų2 �̹���
    public TextMeshProUGUI skill1CoolTime;  // ��ų1 ��Ÿ�� ǥ��
    public TextMeshProUGUI skill2CoolTime;  // ��ų2 ��Ÿ�� ǥ��
    public Transform statusEffect;    // ���� UI�̹��� ������ġ

    protected override void Awake()
    {
        base.Awake();
        WeaponManager.OnSwichWeapon += SetskillImage;
    }
    
    void Update()
    {
        //SetskillImage();
        SetSkillCoolTime();
    }

    public void SetskillImage() // ���� ��ų �̹��� ���� �ֱ� �̰Ŵ� ���߿� ���ⱳü �ً� �ѹ��� ���� �ϱ������ �ϴ� Update�� �׽�Ʈ ����
    {
        weaponImage.sprite = WeaponManager.activeWeapon.weaponScriptable.weaponImage;
        skillImage1.sprite = WeaponManager.activeWeapon.weaponScriptable.skillImage1;
        skillImage2.sprite = WeaponManager.activeWeapon.weaponScriptable.skillImage2;
        
    }

    void SetSkillCoolTime() // ��Ÿ�� ǥ��
    {
        if (WeaponManager.activeWeapon == null)
            return;

        Mathf.Clamp01(skillImage1.fillAmount = WeaponManager.skill1CoolTimedown / WeaponManager.activeWeapon.weaponScriptable.skill1Cool);
        Mathf.Clamp01(skillImage2.fillAmount = WeaponManager.skill2CoolTimedown / WeaponManager.activeWeapon.weaponScriptable.skill2Cool);

        if (WeaponManager.isSkill1Ready)
        {
            skill1CoolTime.enabled = false;
        }
        else
        {
            skill1CoolTime.enabled = true;
            float coolTime1 = WeaponManager.activeWeapon.weaponScriptable.skill1Cool - WeaponManager.skill1CoolTimedown;
            skill1CoolTime.text = coolTime1.ToString("0.0");
        }
        if (WeaponManager.isSkill2Ready)
        {
            skill2CoolTime.enabled = false;
        }
        else
        {
            skill2CoolTime.enabled = true;
            float coolTime2 = WeaponManager.activeWeapon.weaponScriptable.skill2Cool - WeaponManager.skill2CoolTimedown;
            skill2CoolTime.text = coolTime2.ToString("0.0");
        }
    }

    
}
