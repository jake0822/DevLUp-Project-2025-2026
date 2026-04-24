using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public PauseManager PauseManager;
    public Slider VolumeSlider;

    public void AdjustVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    public void Resume()
    {
        PauseManager.SetPause(false);
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
