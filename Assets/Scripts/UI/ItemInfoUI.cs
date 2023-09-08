using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoUI : ItemUI
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    // 사용이나 팔거나 하면 전체 InfoUI게임오브젝트를 잠시꺼두고 하면 될거같다.
    public void UpdateItemInfo(Item item)
    {
        nameText.text = item.itemName;
        descriptionText.text = item.description;
    }
}
