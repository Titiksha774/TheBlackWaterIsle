using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;
    public GameObject hudPanel;
    public GameObject creditsPanel;
    public GameObject storyPanel;
    public GameObject deathPanel;
    public GameObject winPanel;
    public GameObject messagePanel; 
    public TMPro.TextMeshProUGUI messageText; 

    [Header("Audio")]
    public AudioSource menuMusic;

    private bool isPaused = false;

    private void Awake()
    {
        
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    void Start()
    {
        ShowMainMenu();

        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // ---- MAIN MENU ----
    public void ShowMainMenu()
    {
        if (mainMenuPanel == null) return;

        mainMenuPanel.SetActive(true);
        pauseMenuPanel?.SetActive(false);
        hudPanel?.SetActive(false);
        creditsPanel?.SetActive(false);
        storyPanel?.SetActive(false);
        deathPanel?.SetActive(false);
        winPanel?.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (menuMusic != null && !menuMusic.isPlaying)
            menuMusic.Play();
    }

    
    public void PlayGame()
    {
        
        PlayerPrefs.DeleteKey("FactoryActivated");
        PlayerPrefs.Save();

        if (GameManager.Instance != null)
            GameManager.Instance.hasPressedButton = false;

        mainMenuPanel?.SetActive(false);
        storyPanel?.SetActive(true);
        pauseMenuPanel?.SetActive(false);
        hudPanel?.SetActive(false);
        creditsPanel?.SetActive(false);
        deathPanel?.SetActive(false);
        winPanel?.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("✅ Game reset: PlayerPrefs cleared and game state reset.");
    }

    
    public void ContinueAfterStory()
    {
        storyPanel?.SetActive(false);
        mainMenuPanel?.SetActive(false);
        pauseMenuPanel?.SetActive(false);
        hudPanel?.SetActive(true);
        creditsPanel?.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (menuMusic != null)
            menuMusic.Stop();
    }

    
    public void PauseGame()
    {
        pauseMenuPanel?.SetActive(true);
        hudPanel?.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        pauseMenuPanel?.SetActive(false);
        hudPanel?.SetActive(true);

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    public void ShowDeathPanel()
    {
        
        Time.timeScale = 0f;
        isPaused = true;

        deathPanel?.SetActive(true);
        hudPanel?.SetActive(false);
        pauseMenuPanel?.SetActive(false);
        winPanel?.SetActive(false);

        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        
        StartCoroutine(ForceCursorVisible());
    }

    
    private System.Collections.IEnumerator ForceCursorVisible()
    {
        for (int i = 0; i < 15; i++)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            yield return null;
        }
    }

    public void ShowWinPanel()
    {
        Time.timeScale = 0f;
        winPanel?.SetActive(true);

        hudPanel?.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    
    public void ShowMessage(string text, float duration = 3f)
    {
        if (Instance == null) return;
        if (messagePanel == null) return;

        if (messageText == null)
            messageText = messagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        if (messageText != null)
            messageText.text = text;

        messagePanel.SetActive(true);
        CancelInvoke(nameof(HideMessage));
        Invoke(nameof(HideMessage), duration);
    }

    private void HideMessage()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    
    public void ShowCredits()
    {
        creditsPanel?.SetActive(true);
        mainMenuPanel?.SetActive(false);
        pauseMenuPanel?.SetActive(false);
        hudPanel?.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void HideHUD()
    {
        hudPanel?.SetActive(false);
    }
}
