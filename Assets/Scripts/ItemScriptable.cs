using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemScriptable : ScriptableObject
{
    public int number;
    public ItemType itemType;
    public string itemName;
    public string description;

    public Sprite itemImage;  // �����̹����� �޴°�
    public int stackMax = 99; // �ִ� ������ 99���� �����ߴ�.
    [Tooltip("���� ������ų �߰��Ұ� ���ݷ��̳� ���� �Ѿ� ������ ������ ��")]
    public int value;         // ���� ������ų �߰��Ұ� ���ݷ��̳� ���� �Ѿ� ���� ���
    [Tooltip("����ȿ���� ���� �����۸� �̿��ϱ� ���� ���ӽð�")]
    public float buffTime;    // ���� ���ӽð�
    public int sellPrice;     // �Ǹ� ����
}
