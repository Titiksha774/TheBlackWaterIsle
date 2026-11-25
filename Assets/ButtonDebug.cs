using UnityEngine;

public class ButtonDebug : MonoBehaviour
{
    // Attach this to the same GameObject as the Button (or any object)
    public string nameTag = "PlayButtonTest";

    public void OnClicked()
    {
        Debug.Log($"ButtonDebug: {nameTag} clicked at time {Time.realtimeSinceStartup}");
    }
}
