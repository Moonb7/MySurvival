using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject inventoryUI;

    [SerializeField]
    private string mainScene = "MainScene";
    [SerializeField]
    private string mainMenu = "MainMenu";
    [SerializeField]
    private SceneFader fader;
    private bool isPause;

    // Input Mnaget에서 실행
    public void togle()
    {
        isPause = !isPause;
        if (isPause)
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            //EnemyManager.Instance.PauseEnemies();
        }
        else
        {
            pauseUI.SetActive(false);
            inventoryUI.SetActive(false);
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.Locked;
            //EnemyManager.Instance.ResumeEnemies();
        }

    }

    public void OnInventoryButton()
    {
        inventoryUI.SetActive(true);
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
