using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonLogic : MonoBehaviour
{
    public GameObject controlPopUp;
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void showControls() {
        controlPopUp.SetActive(true);
    }
    
    public void hideControls() {
        controlPopUp.SetActive(false);
    }
    

    public void RunCredits()
    {
        SceneManager.LoadScene("Credits Scene");
    }

    public void newGame()
    {
        PlayerPrefs.DeleteAll();
    }

  
}
