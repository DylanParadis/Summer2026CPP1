using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button pauseQuitButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button returnToMenuButton;
    [SerializeField] private Button backButton;

    [Header("In Game UI")]
    [SerializeField] private TMP_Text livesText;

    [Header("Menu References")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject pauseMenu;

    private bool isPaused = false;

    private void Start()
    {
        if (startButton)
            startButton.onClick.AddListener(GameManager.Instance.StartGame);

        if (settingsButton)
            settingsButton.onClick.AddListener(
                () => SetMenu(settingsMenu, mainMenu)
            );

        if (quitButton)
            quitButton.onClick.AddListener(QuitGame);

        if (pauseQuitButton)
            pauseQuitButton.onClick.AddListener(QuitGame);

        if (resumeButton)
            resumeButton.onClick.AddListener(ResumeGame);

        if (returnToMenuButton)
            returnToMenuButton.onClick.AddListener(
                () => ChangeScene("1.Title")
            );

        if (backButton)
            backButton.onClick.AddListener(
                () => SetMenu(mainMenu, settingsMenu)
            );

        if (livesText)
        {
            GameManager.Instance.OnLivesChanged += UpdateLivesText;
            UpdateLivesText(GameManager.Instance.Lives);
        }
    }

    private void Update()
    {
        if (!pauseMenu)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        isPaused = false;

        if (pauseMenu)
            pauseMenu.SetActive(false);

        Time.timeScale = 1f;
    }

    private void ChangeScene(string sceneName)
    {
        // Never carry a paused time scale into another scene.
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene(sceneName);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetMenu(
        GameObject menuToActivate,
        GameObject menuToDeactivate
    )
    {
        if (menuToActivate != null)
            menuToActivate.SetActive(true);

        if (menuToDeactivate != null)
            menuToDeactivate.SetActive(false);
    }

    private void UpdateLivesText(int lives)
    {
        if (livesText)
            livesText.text = "Lives: " + lives;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLivesChanged -= UpdateLivesText;
        }
    }
}