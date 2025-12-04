using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseButtons : MonoBehaviour
{
    public PauseManager PauseManager;
    private void Start()
    {
       
    }
    public void LevelSelect()
    {
        Time.timeScale = 1;
        PauseManager.isPaused = false;
        SceneManager.LoadScene(PauseManager.levelSelectSceneName);
        
    }

    public void play()
    {
        PauseManager.PauseOrResumeGame(new InputAction.CallbackContext());
    }
}
