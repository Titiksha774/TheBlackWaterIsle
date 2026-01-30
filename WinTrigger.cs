using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WinTrigger : MonoBehaviour
{
    public string playerTag = "Player";
    private bool triggered = false;

    void Start()
    {
        Collider c = GetComponent<Collider>();
        if (c != null && !c.isTrigger)
            c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag(playerTag)) return;

        
        bool pressed = false;
        if (GameManager.Instance != null)
            pressed = GameManager.Instance.hasPressedButton;

        if (PlayerPrefs.GetInt("FactoryActivated", 0) == 1)
            pressed = true;

        if (pressed)
        {
            triggered = true;
            UIManager.Instance?.HideHUD();
            UIManager.Instance?.ShowWinPanel();
        }
        else
        {
            UIManager.Instance?.ShowMessage("You must activate the factory button first!", 3f);
        }
    }
}
