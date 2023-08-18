using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : Singleton<SkillUI>
{
    public Image weaponImage;               // 장착 무기이미지
    public Image skillImage1;               // 스킬1 이미지
    public Image skillImage2;               // 스킬2 이미지
    public TextMeshProUGUI skill1CoolTime;  // 스킬1 쿨타임 표시
    public TextMeshProUGUI skill2CoolTime;  // 스킬2 쿨타임 표시
    public Transform statusEffect;    // 버프 UI이미지 생성위치

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

    public void SetskillImage() // 각각 스킬 이미지 집어 넣기 이거는 나중에 무기교체 줄떄 한번만 실행 하기로하자 일단 Update로 테스트 진행
    {
        weaponImage.sprite = WeaponManager.activeWeapon.weaponScriptable.weaponImage;
        skillImage1.sprite = WeaponManager.activeWeapon.weaponScriptable.skillImage1;
        skillImage2.sprite = WeaponManager.activeWeapon.weaponScriptable.skillImage2;
        
    }

    void SetSkillCoolTime() // 쿨타임 표시
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
