using UnityEngine;

// ���⿡ �ʿ��� �ɼ��� �ܺο��� �����Ҽ� �ְ� �����
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponScriptable : ScriptableObject // �� Ŭ������ ���⸶���� Ư¡ ������ �Է��ϱ�����
{
    public WeaponType attackType;           // ���� Ÿ��
    public string weaponName;               // ���� �̸�
    public string weapondescription;        // ���� ����
    public Sprite weaponImage;              // ���� �̹���
    public Sprite skillImage1;              // ��ų1�̹���
    public Sprite skillImage2;              // ��ų2�̹���

    public float atk;                       // �ش� ���� �����
    public float rateSpeed;                 // �⺻ ���� �ӵ�
}
public enum WeaponType
{
    meleeweapon,
    rangedweapon,
}
