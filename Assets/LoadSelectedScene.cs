using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSelectedScene : MonoBehaviour
{
    public string levelName;
    public string sceneName;
    public TextMeshProUGUI text;

    private void Start()
    {
        if(text == null)
            text.text = levelName;
    }


    public void pushButton()
    {
        if (SceneManager.GetSceneByName(sceneName) != null)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            print("No scene with name: " +  sceneName);
        }

    }
}
