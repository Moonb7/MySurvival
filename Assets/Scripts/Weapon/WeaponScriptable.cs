using UnityEngine;

// 무기에 필요한 옵션을 외부에서 설정할수 있게 만들기
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponScriptable : ScriptableObject // 이 클래스는 무기마다의 특징 정보들 입력하기위해
{
    public WeaponType attackType;           // 무기 타입
    public short weaponNum;                 // 무기 번호
    public string weaponName;               // 무기 이름
    public string weapondescription;        // 무기 설명
    public Sprite weaponImage;              // 무기 이미지
    public Sprite skillImage1;              // 스킬1이미지
    public Sprite skillImage2;              // 스킬2이미지

    public float atk;                       // 해당 무기 대미지
    public float chargingEnergyTime;        // 차징공격을 하기위해 모아야할 시간
    public float skill1Cool;                // 스킬 쿨
    public float skill2Cool;                // 스킬 쿨
    public float buffValue;                 // 버프값 올릴값
    public float buffTime;                  // 버프지속시간 각 무기느에 버프스킬은 하나씩만 있는걸로
}
public enum WeaponType
{
    meleeweapon,
    rangedweapon,
}
