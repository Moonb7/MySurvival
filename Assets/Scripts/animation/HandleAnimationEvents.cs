using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 애니메이션 이벤트에 쓸 함수를 관리 하는 클래스
public class HandleAnimationEvents : MonoBehaviour
{
    // 구르기 애니매이션 끝날때쯤 이벤트 함수를 썻다
    public void FinishedRoll()
    {
        PlayerNewInputController.hasroll = false;
    }
}
