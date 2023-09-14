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

    private void Start()
    {
        OptionUI.Instance.optionButton.SetActive(false);
    }
    // Input Mnaget���� ����
    public void togle()
    {
        isPause = !isPause;
        if (isPause)
        {
            pauseUI.SetActive(true);
            if (OptionUI.Instance.gameObject != null)
                 OptionUI.Instance.optionButton.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            //EnemyManager.Instance.PauseEnemies();
        }
        else
        {
            pauseUI.SetActive(false);
            inventoryUI.SetActive(false);
            if(OptionUI.Instance.gameObject != null)
            {
                OptionUI.Instance.CloseUI();
                OptionUI.Instance.optionButton.SetActive(false);
            }
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
        OptionUI.Instance.optionButton.SetActive(true);
        fader.FadeTo(mainMenu);
    }
}
