using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    private string mainScene = "MainScene";

    [SerializeField]
    private string mainMenu = "MainMenu";

    [SerializeField]
    private SceneFader fader;

    [SerializeField]
    private GameObject pauseUI;
    
    private bool isPause;

    // Input Mnaget에서 실행
    public void togle()
    {

        isPause = !isPause;
        if (isPause)
        {
            pauseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        else
        {
            pauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }

    }

    public void OnResumeButton()
    {
        togle();
    }

    public void OnReStartButton()
    {
        togle();
        fader.FadeTo(mainScene);
    }

    public void OnMenuButton()
    {
        togle();
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f;
        fader.FadeTo(mainMenu);
    }


}
