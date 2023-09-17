using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    public SceneFader fader;
    public string mainMenuScene = "MainMenu";

    public float creditsTime;
    private float countDown;

    private void Update()
    {
        countDown += Time.deltaTime;
        if (countDown > 7 && Input.anyKeyDown) // ũ������ �����ϰ� 7�������� ������ �ƹ�Ű�� ���� �� �Ѿ��
        {
            GoMainMenuScene();
        }
        else if (countDown > creditsTime) // ũ���� �ִϸ��̼��� ������ ���� �뿡 �Ѿ�� �����
        {
            GoMainMenuScene();
        }
    }

    private void GoMainMenuScene()
    {
        fader.FadeTo(mainMenuScene);
    }
}
