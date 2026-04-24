using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLogic : MonoBehaviour
{
    public GameObject controlPopUp;
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
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

  
}
