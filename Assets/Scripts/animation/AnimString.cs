using UnityEngine;

public class AnimString : Singleton<AnimString>
{
    // ���� �ִϸ��̼� �Ķ���� �̸�
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
    public string sprint = "Sprint";
    public string roll = "Roll";
    public string jump = "Jump";
    public string attack = "Attack";
    public string attackMode = "AttackMode";
    public string weaponNum = "WeaponNum";      // ���� ��ȣ
    public string fasten = "Fasten";
    public string attackCombo = "AttackCombo";
    public string attackStats = "AttackStats"; // ���� ���� �Ϲݰ������� ��¡�������� �����ϱ�����


    // ���� ����
    public string weaponChange = "WeaponChange";
    public string isMounting = "IsEquip";

    [Header("Enemy")]
    public string isClose = "IsClose";
    public string enemyState = "EnemyState";

}
