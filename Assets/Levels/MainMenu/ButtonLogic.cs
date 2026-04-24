using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLogic : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void RunCredits()
    {
        SceneManager.LoadScene("Credits Scene");
    }

    public void ShowControls()
    {
        SceneManager.LoadScene("Credits Scene");
    }
}
