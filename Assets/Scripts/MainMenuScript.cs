using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Declare public variables to track button presses
    public bool playButtonPressed;
    public bool newGameButtonPressed;
    public bool creditsButtonPressed;

    private void OnMouseUp()
    {
        if (playButtonPressed)
        {
            // Load the level select scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("../Levels/Hub Level/Scenes/Main");
        }
        else if (newGameButtonPressed)
        {
            // Load the first level of the game
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
        }
        else if (creditsButtonPressed)
        {
            // Load the credits scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
        }
    }

    public void OnPlayButtonPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("../Levels/Hub Level/Scenes/Main");
    }

    public void OnCreditsButtonPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
