using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 무기에 필요한 옵션을 외부에서 설정할수 있게 만들기
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponScriptable : ScriptableObject
{
    // 무기가 만들어 졌을때 사용되는 프리팹
    public GameObject SourcePrefab;

    public WeaponAttackType attackType;

    //new public string name;
    public int nuber;

    public Sprite weaponImage;
    public Sprite skillImage1;
    public Sprite skillImage2;

    public float atk;    // 데미지

}
