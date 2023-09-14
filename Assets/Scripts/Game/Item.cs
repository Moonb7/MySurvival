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

    public string itemRoute;  // �����Ϳ��� ��θ� �����°� ���������� �̹����� ������ �ʴ´� XML������ �̿��Ͽ� ItemDataManagerŬ�������� �ޱ����� ����.
    public Sprite itemImage;  // �����̹����� �޴°�
    
    public int amount;
    public int stackMax = 99; // �ִ� ������ 99���� �����ߴ�.
    public int value;         // ���� ������ų �߰��Ұ� ���ݷ��̳� ���� �Ѿ� ���� ���
    public float buffTime;    // ���� ���ӽð�
    public int sellPrice;     // �Ǹ� ����

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

        if (IsStackable() == true)  // ���� �� ������ amount 1��
            amount = 1;
        else
            amount = -1;
    }

    public Item(Item itemDate) // json�̳� xml����� ��������
    {
        number = itemDate.number;
        itemType = itemDate.itemType;
        itemName = itemDate.itemName;
        description = itemDate.description;
        itemRoute = itemDate.itemRoute;
        itemImage = Resources.Load<Sprite>(itemDate.itemRoute); // ��θ� ���� �̹��� ����
        value = itemDate.value;
        buffTime = itemDate.buffTime;
        sellPrice = itemDate.sellPrice;

        if (IsStackable() == true)  // ���� �� ������ amount 1��
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
                DataManager.Instance.AddGold(value); // ��� ȹ��
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
    

    public bool IsStackable() // ������ �ִ� ���������� Ȯ��
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
    Potion,           // ü������ �� �������� ������ ���� �� ���� �κ��丮 ��ĭ�� �����ϰ� ��ȹ
    Consumable,       // �Ҹ�ǰ �Ѿ˹ڽ��� ���ڽ� �Ҹ�ǰ�� ���� �� �ְ� ������
}

public enum ItemName
{
    AmmoBox,
    Gold,
    HPPotion,
    AttackBuffPotion,
    DefenceBuffPotion,
}