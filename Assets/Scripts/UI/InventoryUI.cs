using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryUI : ItemUI
{
    public Transform itemSlotsParent;
    public ItemSlot[] itemSlots;

    private int selectIndex = -1;       // �ε����� 0���� �������� �־ �̷��� ����

    private ItemInfoUI itemInfoUI;

    private void Start()
    {
        itemSlots = itemSlotsParent.GetComponentsInChildren<ItemSlot>();
        itemInfoUI = GetComponent<ItemInfoUI>();
        Inventory.Instance.OnSetInven += UpdateInventoryUI;
        Inventory.Instance.OnSetInven?.Invoke();
    }

    public override void CloseUI()
    {
        isOpen = false;
        thisUI.gameObject.SetActive(isOpen);
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].ResetItemSlot();                             // ������ü �ʱ�ȭ
        }

        for (int i = 0; i < Inventory.Instance.items.Count; i++)
        {
            itemSlots[i].SetItemSlot(Inventory.Instance.items[i], i); // �ٽ� ����

            /*if (itemSlots[i].item == null)
            {
                itemSlots[i].gameObject.SetActive(false);
            }
            else
            {
                itemSlots[i].gameObject.SetActive(true);
            }*/
        }
        itemSlots = itemSlotsParent.GetComponentsInChildren<ItemSlot>();
    }

    public void SelectSlot(int slotIndex)
    {
        if(selectIndex == slotIndex) // ���� ������ Ŭ���ϸ� infoUI�ݱ�
        {
            DeselectSlot(true);
            return;
        }
        else
        {
            DeselectSlotAll(); // �ٸ� ������ ���õɶ��� ��� ���� ��Ȱ��ȭ
        }
        selectIndex= slotIndex; 

        itemSlots[selectIndex].selectImage.SetActive(true); // ������ ���� Ȱ��ȭ

        itemInfoUI.OpenUI(); // InfoUI ����
        itemInfoUI.UpdateItemInfo(Inventory.Instance.items[selectIndex]); // �ش� ������ ���� InfoUI�� ǥ��
    }

    public void DeselectSlot(bool isInfoUI) // ������ ����â �ݱ�
    {
        if (selectIndex < 0)
            return;

        itemSlots[selectIndex].selectImage.SetActive(false);
        selectIndex = -1;

        //������ ����â �ݱ�
        if (isInfoUI == true)
        {
            itemInfoUI.CloseUI();
        }
    }

    void DeselectSlotAll()
    {
        for(int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].selectImage.SetActive(false);
        }
        selectIndex = -1;
    }

    public void ItemUse()
    {
        //��� ȿ�� ����
        Debug.Log(Inventory.Instance.items[selectIndex].itemName + "�������� ����Ͽ����ϴ�");
        Inventory.Instance.items[selectIndex].Use();

        //������ �������� �κ��丮 ����Ʈ���� ����
        Inventory.Instance.RemoveItem(Inventory.Instance.items[selectIndex]);
        DeselectSlot(true);
    }

    public void ItemSell()
    {
        //�Ǹ� ���� ����
        PlayerStats.Instance.AddGold(Inventory.Instance.items[selectIndex].sellPrice);
        Debug.Log("TODO: " + Inventory.Instance.items[selectIndex].itemName + "�������� �Ⱦҽ��ϴ�");

        //������ �������� �κ��丮 ����Ʈ���� ����
        Inventory.Instance.RemoveItem(Inventory.Instance.items[selectIndex]);
        DeselectSlot(true);
    }
}
