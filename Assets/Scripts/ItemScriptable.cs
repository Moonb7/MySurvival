using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemScriptable : ScriptableObject
{
    public int number;
    public ItemType itemType;
    public string itemName;
    public string description;

    public Sprite itemImage;  // 직접이미지를 받는것
    public int stackMax = 99; // 최대 수량은 99개로 지정했다.
    [Tooltip("무언가 증가시킬 추가할값 공격력이나 방어력 총알 갯수등 증가할 값")]
    public int value;         // 무언가 증가시킬 추가할값 공격력이나 방어력 총알 갯수 등등
    [Tooltip("버프효과를 가진 아이템만 이용하기 버프 지속시간")]
    public float buffTime;    // 버프 지속시간
    public int sellPrice;     // 판매 가격
}
