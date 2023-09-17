using System.IO;
using UnityEngine;

public class DataManager : PersistentSingleton<DataManager>
{
    public PlayerData playerData = new PlayerData();

    [HideInInspector]
    public string path;

    protected override void Awake()
    {
        base.Awake();
        path = Application.persistentDataPath + "/saveFile";
        if (File.Exists(path))
        {
            LoadData();
        }
    }

    public void SaveData() // ������ ����
    {
        string data = JsonUtility.ToJson(playerData);
        File.WriteAllText(path, data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path); // ������ �а�
        playerData = JsonUtility.FromJson<PlayerData>(data); // �ٽ� ���� ���·� ��ȯ
    }

    public void DataClear()
    {
        playerData = new PlayerData();
    }

    public void AddGold(int amount) // ��� ȹ��
    {
        playerData.gold += amount;
        Debug.Log($"Player�� ���� �ִ� ��差 : {playerData.gold}");
    }
    public bool UseGold(int amount) // ��� ���
    {
        if (playerData.gold < amount)
            return false;

        playerData.gold -= amount;
        return true;
    }

    public void AddAmmor(int amount)
    {
        playerData.ammoCount += amount;
    }

    public bool UseAmmor(int amount)
    {
        if (playerData.ammoCount < amount)
            return false;

        playerData.ammoCount -= amount;
        return true;
    }
}
