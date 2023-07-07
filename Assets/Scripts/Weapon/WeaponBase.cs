using UnityEngine;

// 무기 오브젝트 프리팹에 넣을 컴포넌트 클래스이다. WeaponController 이다.
public abstract class WeaponBase : MonoBehaviour
{
    public WeaponScriptable weaponScriptable;



    protected abstract void Skill1();
    protected abstract void Skill2();
}
