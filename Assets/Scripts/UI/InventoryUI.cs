using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : ItemUI
{
    private Inventory inventory;

    public Transform itemSlotsParent;
    private ItemSlot[] itemSlots;

    private int selecIndex = -1;       // �ε����� 0���� �������� �־ �̷��� ����

    

    public void SelectSlot(int slotIndex)
    {

    }
}
