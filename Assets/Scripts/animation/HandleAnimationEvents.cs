using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ִϸ��̼� �̺�Ʈ�� �� �Լ��� ���� �ϴ� Ŭ����
public class HandleAnimationEvents : MonoBehaviour
{
    // ������ �ִϸ��̼� �������� �̺�Ʈ �Լ��� ����
    public void FinishedRoll()
    {
        PlayerNewInputController.hasroll = false;
    }
}
