using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �����͸� ��� �ִ� ��ũ��Ʈ
public class ItemDataManager : PersistentSingleton<ItemDataManager>
{
    public List<Item> items = new List<Item>();

    public List<ItemScriptable> itemScriptables = new List<ItemScriptable>();

    protected override void Awake()
    {
        base.Awake();

        for(int i = 0; i < itemScriptables.Count; i++) // ��ϵ� ������ �� ��ŭ
        {
            Item item = new Item(itemScriptables[i]);

            items.Add(item);
        }
    }
}
