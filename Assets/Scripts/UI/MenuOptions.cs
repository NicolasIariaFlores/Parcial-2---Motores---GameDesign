using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class MenuOptions : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public void FullScreen(bool _fullScreen)
    {
        Screen.fullScreen = _fullScreen;
    }

    public void ChangeVolume(float volume)
    {
        audioMixer.SetFloat("Volumen", volume);
    }

    public void ChangeQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

}
