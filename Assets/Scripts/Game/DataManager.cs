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

    public void SaveData() // 데이터 저장
    {
        string data = JsonUtility.ToJson(playerData);
        File.WriteAllText(path, data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path); // 파일을 읽고
        playerData = JsonUtility.FromJson<PlayerData>(data); // 다시 원래 형태로 변환
    }

    public void DataClear()
    {
        playerData = new PlayerData();
    }

    public void AddGold(int amount) // 골드 획득
    {
        playerData.gold += amount;
        Debug.Log($"Player가 갖고 있는 골드량 : {playerData.gold}");
    }
    public bool UseGold(int amount) // 골드 사용
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
