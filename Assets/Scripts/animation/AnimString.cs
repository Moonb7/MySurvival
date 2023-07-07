using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimString : Singleton<AnimString>
{
    // 공통 애니매이션 파라미터
    public string hit = "Hit";
    public string isDie = "IsDie";
    public string isAttack = "IsAttack";
    public string canMove = "CanMove";

    [Header("Player")]
    public string speed = "Speed";
    public string X = "X";
    public string Y = "Y";
    public string isGround = "IsGround";
    public string move = "Move";
    public string roll = "Roll";
    public string jump = "Jump";
    public string attack = "Attack";
    public string attackMode = "AttackMode";
    public string weaponStats = "WeaponStats"; // Weaon타입
    public string fasten = "Fasten";
    
    // 무기 관련
    public string weaponChange = "WeaponChange";
    public string isMounting = "IsMounting";

    [Header("Enemy")]
    public string enemyState = "EnemyState";

}
