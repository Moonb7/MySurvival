using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int[] upgradeLevel = new int[3]; // 각 강화레벨

    public int gold = 0;
    public int ammoCount = 0;

    public float maxHealth = 100;
    public float attack = 5;
    public float defence = 0;

    public bool isGameClear = false;
}
