using UnityEngine;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    public Slider sliderVolumeMusica;
    public Slider sliderVolumeEffetti;

    void Start()
    {
        // Collega gli slider agli eventi
        sliderVolumeMusica.onValueChanged.AddListener(SetVolumeMusica);
        sliderVolumeEffetti.onValueChanged.AddListener(SetVolumeEffetti);
    }

    void SetVolumeMusica(float volume)
    {
        // In questo esempio usiamo AudioListener per semplicità
        AudioListener.volume = volume;
        Debug.Log("🎵 Volume musica: " + volume);
    }

    void SetVolumeEffetti(float volume)
    {
        // Puoi collegarlo a un AudioMixer se lo userai
        Debug.Log("🔊 Volume effetti: " + volume);
    }
}
