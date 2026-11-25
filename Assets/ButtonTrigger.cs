using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public GameObject messagePanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.hasPressedButton = true;
            UIManager.Instance.ShowMessage("Button activated! The lighthouse door is now open!");
        }
    }
}
