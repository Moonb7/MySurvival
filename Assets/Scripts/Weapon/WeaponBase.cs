using UnityEngine;

// ���� ������Ʈ �����տ� ���� ������Ʈ Ŭ�����̴�. WeaponController �̴�.
public abstract class WeaponBase : MonoBehaviour
{
    public WeaponScriptable weaponScriptable;



    protected abstract void Skill1();
    protected abstract void Skill2();
}
