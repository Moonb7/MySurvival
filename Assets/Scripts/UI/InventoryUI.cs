using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryUI : ItemUI
{
    public Transform itemSlotsParent;
    public ItemSlot[] itemSlots;

    private int selectIndex = -1;       // 인덱스가 0부터 아이템이 있어서 이렇개 설정

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
            itemSlots[i].ResetItemSlot();                             // 슬롯전체 초기화
        }

        for (int i = 0; i < Inventory.Instance.items.Count; i++)
        {
            itemSlots[i].SetItemSlot(Inventory.Instance.items[i], i); // 다시 셋팅

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
        if(selectIndex == slotIndex) // 같은 슬롯을 클릭하면 infoUI닫기
        {
            DeselectSlot(true);
            return;
        }
        else
        {
            DeselectSlotAll(); // 다른 슬롯이 선택될때는 모든 슬롯 비활성화
        }
        selectIndex= slotIndex; 

        itemSlots[selectIndex].selectImage.SetActive(true); // 선택한 슬롯 활성화

        itemInfoUI.OpenUI(); // InfoUI 열기
        itemInfoUI.UpdateItemInfo(Inventory.Instance.items[selectIndex]); // 해당 아이템 정보 InfoUI에 표시
    }

    public void DeselectSlot(bool isInfoUI) // 아이템 인포창 닫기
    {
        if (selectIndex < 0)
            return;

        itemSlots[selectIndex].selectImage.SetActive(false);
        selectIndex = -1;

        //아이템 정보창 닫기
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
        //사용 효과 구현
        Debug.Log(Inventory.Instance.items[selectIndex].itemName + "아이템을 사용하였습니다");
        Inventory.Instance.items[selectIndex].Use();

        //선택한 아이템을 인벤토리 리스트에서 제거
        Inventory.Instance.RemoveItem(Inventory.Instance.items[selectIndex]);
        DeselectSlot(true);
    }

    public void ItemSell()
    {
        //판매 보상 구현
        PlayerStats.Instance.AddGold(Inventory.Instance.items[selectIndex].sellPrice);
        Debug.Log("TODO: " + Inventory.Instance.items[selectIndex].itemName + "아이템을 팔았습니다");

        //선택한 아이템을 인벤토리 리스트에서 제거
        Inventory.Instance.RemoveItem(Inventory.Instance.items[selectIndex]);
        DeselectSlot(true);
    }
}
