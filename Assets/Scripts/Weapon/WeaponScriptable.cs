using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// ���⿡ �ʿ��� �ɼ��� �ܺο��� �����Ҽ� �ְ� �����
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponScriptable : ScriptableObject
{
    // ���Ⱑ ����� ������ ���Ǵ� ������
    public GameObject SourcePrefab;

    public WeaponAttackType attackType;

    //new public string name;
    public int nuber;

    public Sprite weaponImage;
    public Sprite skillImage1;
    public Sprite skillImage2;

    public float atk;    // ������

}
