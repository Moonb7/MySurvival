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

    public void AddGold(int amount) // °ñµå È¹µæ
    {
        Gold += amount;
        Debug.Log($"Player°¡ °®°í ÀÖ´Â °ñµå·® : {Gold}");
    }
    public bool UseGold(int amount) // °ñµå »ç¿ë
    {
        if (Gold < amount)
            return false;

        Gold -= amount;
        return true;
    }
}
