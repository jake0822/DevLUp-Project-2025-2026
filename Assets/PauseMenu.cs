using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public PauseManager PauseManager;
    public Slider VolumeSlider;

    public void AdjustVolume()
    {

    }

    public void Resume()
    {
        PauseManager.SetPause(false);
    }
}
