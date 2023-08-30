using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : PersistentSingleton<Inventory>
{
    public List<Item> items = new List<Item>();  // �κ��丮 ������ ����Ʈ

    public int invenMax = 16;                    // �κ��丮 ũ��

    public UnityAction OnSetInven;               // �̺��丮 �˻� �� ������

    public bool IsEmptyInven(Item newItem) // ������ĭ�� �����ִ��� Ȯ��
    {
        bool isEmpty = items.Count < invenMax;

        if(isEmpty == false && newItem.IsStackable()) // �κ��丮�� �� ���� �ش�������� ������ ������ ������
        {
            foreach(Item item in items)
            {
                if(item.number == newItem.number) // �� Ȯ���ؼ� ���� �������� ������
                {
                    if(item.amount < item.stackMax) // �ش� �������� ������ �ʰ����� ������
                    {
                        isEmpty= true;
                        break;
                    }
                }
            }
        }
        return isEmpty;
    }

    public bool AddItem(Item newitem, bool isEquipChange = false) // ���������� Change�ٲپ� �����Ѱ��� Ȯ���ϱ� ���� ���� �ϳ� ����
    {
        if(IsEmptyInven(newitem) == false)
        {
            if(isEquipChange == true) // ������ ��ü�� �Ѱ��� Ȯ��
            {
                items.Add(newitem); // �κ��丮�� �ְ�
                OnSetInven?.Invoke(); // �̺��丮 �˻� �� ������
                return true;
            }

            Debug.Log("Inventory Full!!!");
            return false;
        }

        if(newitem.IsStackable()) // ������ ���� �� �ִ� �������̸�
        {
            bool isFind = false; // ���� �������� �ִ��� üũ��
            foreach(Item item in items) 
            {
                if (item.number == newitem.number) // newItem�� ���� �κ��丮�� �ִ��� ������ �˻�
                {
                    if (item.amount < item.stackMax) // �κ��丮�� �����鼭 stackMax���� ������ �˻�
                    {
                        item.amount++; // �ϳ��� ����
                        isFind = true;
                        break; // �ݺ��� ������
                    }
                }
            }
            if(isFind == false) // ���� �������� ������
            {
                newitem.amount = 1;
                items.Add(newitem);
            }
        }
        else
        {
            items.Add(newitem);
        }

        OnSetInven?.Invoke(); // �̺��丮 �˻� �� ������

        return true;
    }

    public void RemoveItem(Item oldItem)
    {
        if (oldItem.IsStackable()) // �״� �������̸�
        {
            oldItem.amount--;
            if(oldItem.amount <= 0) // 0�� �ϳ��� ������
            {
                items.Remove(oldItem);
            }
        }
        else
        {
            items.Remove(oldItem);
        }

        OnSetInven?.Invoke(); // �̺��丮 �˻� �� ������
    }
}
