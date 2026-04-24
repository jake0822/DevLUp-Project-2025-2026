using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public string levelSelectSceneName;

    public bool isPaused = false;
   
    

    private void Start()
    {

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void SetPause(bool state)
    {
        isPaused = state;
        pauseMenuUI.SetActive(state);
        Time.timeScale = state ? 1.0f : 0;
        Cursor.visible = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void TogglePause()
    {
        SetPause(!isPaused);
    }

    // Connects to the pause event from InputAction.
    public void HandlePauseInput(InputAction.CallbackContext context)
    {
        TogglePause();
    }
}
