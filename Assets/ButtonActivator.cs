using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ButtonActivator : MonoBehaviour
{
    [Header("Settings")]
    public string playerTag = "Player";
    public bool disableAfterPress = true;
    public AudioSource activateSound; 

    [Header("Optional visual indicator (World Space)")]
    public GameObject pressIndicator; 

    bool canPress = false;
    bool isActivated = false;

    void Start()
    {
        
        Collider c = GetComponent<Collider>();
        if (c != null && !c.isTrigger)
            c.isTrigger = true;

        
        if (pressIndicator != null)
            pressIndicator.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag) || isActivated) return;

        canPress = true;

        
        UIManager.Instance?.ShowMessage("Press E to activate the factory button", 3f);
        if (pressIndicator != null) pressIndicator.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        canPress = false;
        if (pressIndicator != null) pressIndicator.SetActive(false);
    }

    void Update()
    {
        if (canPress && !isActivated && Input.GetKeyDown(KeyCode.E))
        {
            ActivateButton();
        }
    }

    void ActivateButton()
    {
        isActivated = true;


        if (GameManager.Instance != null)
            GameManager.Instance.hasPressedButton = true;


        PlayerPrefs.SetInt("FactoryActivated", 1);
        PlayerPrefs.Save();


        UIManager.Instance?.ShowMessage("Button activated! Lighthouse power restored.", 3f);
        if (pressIndicator != null) pressIndicator.SetActive(false);
        if (activateSound != null) activateSound.Play();

        if (disableAfterPress)
        {

            Collider c = GetComponent<Collider>();
            if (c != null) c.enabled = false;

        }
    }
}