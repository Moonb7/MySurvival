using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : ItemUI
{
    private Inventory inventory;

    public Transform itemSlotsParent;
    private ItemSlot[] itemSlots;

    private int selecIndex = -1;       // 인덱스가 0부터 아이템이 있어서 이렇개 설정

    

    public void SelectSlot(int slotIndex)
    {

    }
}
