using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour 
{
    private Item item;

    public GameObject itemImage;
    public GameObject selectImage;
    public TextMeshProUGUI amount; // °¹¼ö

    private int slotIndex;

    private InventoryUI inventoryUI;

    private void Start()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();
    }

    public void SetItemSlot(Item newItem, int _slotIndex)
    {
        if (newItem == null)
            return;
        item = newItem;

        itemImage.SetActive(true);
        itemImage.GetComponent<Image>().sprite = item.itemImage;
        if(item.amount > 0)
        {
            amount.gameObject.SetActive(true); // °¹¼ö Ç¥½Ã
            amount.text = item.amount.ToString();
        }

        slotIndex = _slotIndex;
    }

    public void ResetItemSlot()
    {
        item = null;

        itemImage.SetActive(false);
        item.GetComponent<Image>().sprite = null;
        selectImage.SetActive(false);
        amount.gameObject.SetActive(false);
    }

    public void SelectSlot() // ¼±ÅÃÇ¥½Ã
    {
        if(item == null) 
            return;

        inventoryUI.SelectSlot(slotIndex); 
    }
}
