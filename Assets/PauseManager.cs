using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public string levelSelectSceneName;

    public bool isPaused;
   
    

    private void Start()
    {

        pauseMenuUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1.0f;
    }

    public void PauseOrResumeGame(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            
            isPaused = false;
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else 
        {
            
            isPaused = true;
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
    }
    

}
