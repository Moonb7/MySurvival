using UnityEngine;

// ���⿡ �ʿ��� �ɼ��� �ܺο��� �����Ҽ� �ְ� �����
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponScriptable : ScriptableObject // �� Ŭ������ ���⸶���� Ư¡ ������ �Է��ϱ�����
{
    public WeaponType attackType;           // ���� Ÿ��
    public int weaponNum;                   // ���� ��ȣ
    public string weaponName;               // ���� �̸�
    public string weapondescription;        // ���� ����
    public Sprite weaponImage;              // ���� �̹���
    public Sprite skillImage1;              // ��ų1�̹���
    public Sprite skillImage2;              // ��ų2�̹���

    public float atk;                       // �ش� ���� �����
    public float chargingEnergyTime;        // ��¡������ �ϱ����� ��ƾ��� �ð�
    public float skill1Cool;                // ��ų ��
    public float skill2Cool;                // ��ų ��

    [Header("���� ��ų�� ������ ��� �� ����� �ϳ��� ������ų�� ���� �� �ְ� ��ȹ�ߴ�.")]
    public float buffValue;                 // ������ �ø���
    public float buffTime;                  // �������ӽð� �� ������� ������ų�� �ϳ����� �ִ°ɷ�
    //public float useSkill1Stamina;             // 1�� ��ų ���� �ʿ��� ���¹̳�
    //public float useSkill2Stamina;             // 2�� ��ų ���� �ʿ��� ���¹̳�
}
public enum WeaponType
{
    meleeweapon,
    rangedweapon,
}
