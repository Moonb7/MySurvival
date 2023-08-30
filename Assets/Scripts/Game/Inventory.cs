using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : PersistentSingleton<Inventory>
{
    public List<Item> items = new List<Item>();  // 인벤토리 아이템 리스트

    public int invenMax = 16;                    // 인벤토리 크기

    public UnityAction OnSetInven;               // 이벤토리 검사 및 재정리

    public bool IsEmptyInven(Item newItem) // 아이템칸이 남아있는지 확인
    {
        bool isEmpty = items.Count < invenMax;

        if(isEmpty == false && newItem.IsStackable()) // 인벤토리가 다 차고 해당아이템이 갯수를 쌓을수 있으면
        {
            foreach(Item item in items)
            {
                if(item.number == newItem.number) // 다 확인해서 같은 아이템이 있으면
                {
                    if(item.amount < item.stackMax) // 해당 아이템의 갯수가 초과하지 않으면
                    {
                        isEmpty= true;
                        break;
                    }
                }
            }
        }
        return isEmpty;
    }

    public bool AddItem(Item newitem, bool isEquipChange = false) // 장장착에서 Change바꾸어 장착한건지 확인하기 위해 변수 하나 생성
    {
        if(IsEmptyInven(newitem) == false)
        {
            if(isEquipChange == true) // 아이템 교체를 한건지 확인
            {
                items.Add(newitem); // 인벤토리에 넣고
                OnSetInven?.Invoke(); // 이벤토리 검사 및 재정리
                return true;
            }

            Debug.Log("Inventory Full!!!");
            return false;
        }

        if(newitem.IsStackable()) // 갯수를 쌓을 수 있는 아이템이면
        {
            bool isFind = false; // 같은 아이템이 있는지 체크용
            foreach(Item item in items) 
            {
                if (item.number == newitem.number) // newItem이 현재 인벤토리에 있는지 없는지 검사
                {
                    if (item.amount < item.stackMax) // 인벤토리에 있으면서 stackMax보다 작은지 검사
                    {
                        item.amount++; // 하나씩 증가
                        isFind = true;
                        break; // 반복문 나오고
                    }
                }
            }
            if(isFind == false) // 같은 아이템이 없으면
            {
                newitem.amount = 1;
                items.Add(newitem);
            }
        }
        else
        {
            items.Add(newitem);
        }

        OnSetInven?.Invoke(); // 이벤토리 검사 및 재정리

        return true;
    }

    public void RemoveItem(Item oldItem)
    {
        if (oldItem.IsStackable()) // 쌓는 아이템이면
        {
            oldItem.amount--;
            if(oldItem.amount <= 0) // 0개 하나도 없으면
            {
                items.Remove(oldItem);
            }
        }
        else
        {
            items.Remove(oldItem);
        }

        OnSetInven?.Invoke(); // 이벤토리 검사 및 재정리
    }
}
