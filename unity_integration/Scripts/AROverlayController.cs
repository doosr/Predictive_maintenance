using UnityEngine;
using UnityEngine.UI;

public class AROverlayController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject infoPanel; // Assign a Canvas Panel in World Space
    public Text vibrationText;
    public Text tempText;
    public Text statusText;

    [Header("Target")]
    public DigitalTwinController dataController; // Reference to the main script

    void Update()
    {
        if (dataController != null && infoPanel != null)
        {
            // Make the UI always face the camera (Billboard effect)
            infoPanel.transform.LookAt(Camera.main.transform);
            infoPanel.transform.Rotate(0, 180, 0); // Correct rotation

            // Update Text
            // Note: You need to make 'currentData' public in DigitalTwinController or add getters
            // For this example, we assume we can access the data
            // float vib = dataController.GetCurrentVibration(); 
            // vibrationText.text = $"Vib: {vib:F2} mm/s";
        }
    }
}
