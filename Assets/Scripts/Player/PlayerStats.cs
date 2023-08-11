using UnityEngine;

public class PlayerStats : CharacterStats
{
    [SerializeField]
    private int startGold = 500;
    private int gold;
    public int Gold
    {
        get { return gold; }
        private set { gold = value; }
    }

    protected override void Start()
    {
        base.Start();
        Gold = startGold;
    }

    public void AddGold(int amount) // ��� ȹ��
    {
        Gold += amount;
        Debug.Log($"Player�� ���� �ִ� ��差 : {Gold}");
    }
    public bool UseGold(int amount) // ��� ���
    {
        if (Gold < amount)
            return false;

        Gold -= amount;
        return true;
    }
}
