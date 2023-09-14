using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int number;
    public ItemType itemType;
    public string itemName;
    public string description;

    public string itemRoute;  // 데이터에서 경로만 얻어오는것 직접적으로 이미지를 받지는 않는다 XML파일을 이용하여 ItemDataManager클래스에서 받기위해 썻다.
    public Sprite itemImage;  // 직접이미지를 받는것
    
    public int amount;
    public int stackMax = 99; // 최대 수량은 99개로 지정했다.
    public int value;         // 무언가 증가시킬 추가할값 공격력이나 방어력 총알 갯수 등등
    public float buffTime;    // 버프 지속시간
    public int sellPrice;     // 판매 가격

    public Item()
    {
        number = -1;
    }

    public Item(ItemScriptable itemDate)
    {
        number = itemDate.number;
        itemType = itemDate.itemType;
        itemName = itemDate.itemName;
        description = itemDate.description;
        itemImage = itemDate.itemImage;
        value = itemDate.value;
        buffTime = itemDate.buffTime;
        sellPrice = itemDate.sellPrice;

        if (IsStackable() == true)  // 쌓을 수 있으면 amount 1개
            amount = 1;
        else
            amount = -1;
    }

    public Item(Item itemDate) // json이나 xml고민후 실행하자
    {
        number = itemDate.number;
        itemType = itemDate.itemType;
        itemName = itemDate.itemName;
        description = itemDate.description;
        itemRoute = itemDate.itemRoute;
        itemImage = Resources.Load<Sprite>(itemDate.itemRoute); // 경로를 통해 이미지 적용
        value = itemDate.value;
        buffTime = itemDate.buffTime;
        sellPrice = itemDate.sellPrice;

        if (IsStackable() == true)  // 쌓을 수 있으면 amount 1개
            amount = 1;
        else
            amount = -1;
    }

    public void Use()
    {
        CharacterStats characterStats = GameObject.Find("Player").GetComponent<CharacterStats>();

        ItemName _itemName = (ItemName)number;
        switch (_itemName)
        {
            case ItemName.AmmoBox:
                DataManager.Instance.AddAmmor(value);
                break;
            case ItemName.Gold:
                DataManager.Instance.AddGold(value); // 골드 획득
                break;
            case ItemName.HPPotion:
                characterStats.Heal(value);
                break;
            case ItemName.AttackBuffPotion:
                characterStats.UseBuffItem(this);
                break;
            case ItemName.DefenceBuffPotion:
                characterStats.UseBuffItem(this);
                break;
        }

        /*IEnumerator AttBufPotion()
        {
            characterStats.attack.AddValue(value);
            yield return new WaitForSecondsRealtime(buffTime);
            characterStats.attack.RemoveValue(value);
        }

        IEnumerator DefBufPotion()
        {
            characterStats.defence.AddValue(value);
            yield return new WaitForSecondsRealtime(buffTime);
            characterStats.defence.RemoveValue(value);
        }*/
    }
    

    public bool IsStackable() // 쌓을수 있는 아이템인지 확인
    {
        if (itemType == ItemType.Consumable)
        {
            return true;
        }
        return false;
    }
}

public enum ItemType
{
    Potion,           // 체력포션 및 버프포션 포션은 쌓을 수 없게 인벤토리 한칸씩 차지하게 기획
    Consumable,       // 소모품 총알박스나 골드박스 소모품은 쌓을 수 있게 만들자
}

public enum ItemName
{
    AmmoBox,
    Gold,
    HPPotion,
    AttackBuffPotion,
    DefenceBuffPotion,
}