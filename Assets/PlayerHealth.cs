using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public bool startAtMax = true;
    public float currentHealth = 100f;

    [Header("UI References")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    
    public GameObject deathPanelFallback;

    private bool isDead = false;
    private UIManager uiManager;

    private void Awake()
    {
        if (startAtMax)
            currentHealth = maxHealth;
    }

    private void Start()
    {
        try
        {
            uiManager = FindObjectOfType<UIManager>();

            if (healthSlider == null)
                healthSlider = FindObjectOfType<Slider>();

            if (healthSlider != null)
            {
                healthSlider.minValue = 0f;
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
            }

            UpdateHealthUI();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error initializing health UI: " + ex.Message);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead || amount <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        if (isDead || amount <= 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();
    }

    public void SetMaxHealth(float newMax)
    {
        maxHealth = Mathf.Max(1f, newMax);
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (healthText != null)
            healthText.text = $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player died");

        
        if (uiManager != null)
        {
            uiManager.ShowDeathPanel();
        }
        else
        {
            
            Time.timeScale = 0f;
            if (deathPanelFallback != null)
                deathPanelFallback.SetActive(true);
            else
                Debug.LogWarning("Death occurred but no UIManager or fallback death panel assigned.");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }
}
