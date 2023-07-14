using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ִϸ��̼� �̺�Ʈ�� �� �Լ��� ���� �ϴ� Ŭ����
public class HandleAnimationEvents : MonoBehaviour
{
    public Transform effectGenerator; // ����Ʈ ������ġ

    // ������ �ִϸ��̼� �������� �̺�Ʈ �Լ��� ����
    void FinishedRoll()
    {
        PlayerNewInputController.hasroll = false;
    }

    void InstantiateEffect() // ����Ʈ ����
    {
        GameObject instance = Instantiate(WeaponManager.activeWeapon.attackEffect, effectGenerator); // �⺻ ���� ����Ʈ������Ʈ ����
        Destroy(instance, 5f);
    }
}
