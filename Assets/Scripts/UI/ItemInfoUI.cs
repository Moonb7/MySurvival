using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoUI : ItemUI
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    // ����̳� �Ȱų� �ϸ� ��ü InfoUI���ӿ�����Ʈ�� ��ò��ΰ� �ϸ� �ɰŰ���.
    public void UpdateItemInfo(Item item)
    {
        nameText.text = item.itemName;
        descriptionText.text = item.description;
    }
}
