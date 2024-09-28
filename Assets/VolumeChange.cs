using UnityEngine;
using UnityEngine.UI;

public class VolumeChange : MonoBehaviour
{
    public Slider volumeSlider;  // Assign the UI Slider in the Inspector

    private void Start()
    {
        AudioListener.volume = 0.5f;
        // Set initial slider value to the current global volume level
        volumeSlider.value = AudioListener.volume;

        // Add a listener to handle changes in slider value
        volumeSlider.onValueChanged.AddListener(SetGlobalVolume);
    }

    // Function to set the global volume based on the slider value
    private void SetGlobalVolume(float value)
    {
        AudioListener.volume = value;
    }

    private void OnDestroy()
    {
        // Clean up listener when the script is destroyed
        volumeSlider.onValueChanged.RemoveListener(SetGlobalVolume);
    }
}
