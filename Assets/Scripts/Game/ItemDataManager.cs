using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 데이터만 들어 있는 스크립트
public class ItemDataManager : PersistentSingleton<ItemDataManager>
{
    public List<Item> items = new List<Item>();

    public List<ItemScriptable> itemScriptables = new List<ItemScriptable>();

    protected override void Awake()
    {
        base.Awake();

        for(int i = 0; i < itemScriptables.Count; i++) // 등록된 아이템 수 만큼
        {
            Item item = new Item(itemScriptables[i]);

            items.Add(item);
        }
    }
}
