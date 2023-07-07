using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public Image weaponImage;
    public Image skillImage1;
    public Image skillImage2;

    void Update()
    {
        SetskillInage();
    }

    void SetskillInage()
    {
        weaponImage.sprite = WeaponManager.activeWeapon.weaponScriptable.weaponImage;
        skillImage1.sprite = WeaponManager.activeWeapon.weaponScriptable.skillImage1;
        skillImage2.sprite = WeaponManager.activeWeapon.weaponScriptable.skillImage2;
    }
}
