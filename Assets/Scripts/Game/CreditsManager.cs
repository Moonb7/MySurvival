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
        if (countDown > 7 && Input.anyKeyDown) // 크레딧이 시작하고 7초정도가 지나면 아무키나 누를 시 넘어가기
        {
            GoMainMenuScene();
        }
        else if (countDown > creditsTime) // 크레딧 애니메이션이 끝나는 시점 쯤에 넘어가게 만들기
        {
            GoMainMenuScene();
        }
    }

    private void GoMainMenuScene()
    {
        fader.FadeTo(mainMenuScene);
    }
}
