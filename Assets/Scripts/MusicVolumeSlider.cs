using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;

    private const string VOLUME_KEY = "MusicVolume";

    void Start()
    {
        // Load saved volume or default to 1
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);

        volumeSlider.value = savedVolume;

        // Apply volume to music
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolume(savedVolume);
        }

        // Listen for slider changes
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float value)
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolume(value);
        }

        // Save volume
        PlayerPrefs.SetFloat(VOLUME_KEY, value);
        PlayerPrefs.Save();
    }
}
