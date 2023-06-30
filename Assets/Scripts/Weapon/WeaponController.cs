using UnityEngine;

public enum WeaponAttackType 
{
    Boxing,
    Sword,
    Gun
}
public class WeaponController : MonoBehaviour
{

    // ������ �����ΰ�
    public GameObject Owner { get; set; }

    public WeaponScriptable weapon;

    private WeaponAttackType weaponType;
    public WeaponAttackType WeaponType
    {
        get { return weaponType; }
    }

    private float atk;    // ������
    public float Atk { get { return atk; } set { atk = value; } }

    private void Awake()
    {
        atk = weapon.atk;
        weaponType = weapon.attackType;
    }

    void WeaponTypeMode()
    {
        switch (weaponType)
        {
            case WeaponAttackType.Boxing:
                
                break;
            case WeaponAttackType.Sword:
                
                break;
            case WeaponAttackType.Gun:
                
                break;
        }
    }
}

