using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    public string targetSceneName;
    public TextMeshPro Text;

    public bool portalDeactivate = false;

    private bool inCollider;

    public bool volcanoEnd, CavernEnd, SkyEnd;

   

    private void OnTriggerEnter(Collider other)
    {
        if (portalDeactivate)
        {
            return;
        }
        print(other.tag);   
        if (other.CompareTag("player"))
        {
            Text.enabled = true;
            inCollider = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (portalDeactivate)
        {
            return;
        }
        if (other.CompareTag("player"))
        {
            Text.enabled = false;
            inCollider = false;
        }
    }

    public void LeftClick(InputAction.CallbackContext context) //detects input for movement
    {
        if (inCollider)
        {
            if (volcanoEnd)
            {
                PlayerPrefs.SetInt("VolcanoComplete", 1);
                PlayerPrefs.SetString("MostRecentLevelComplete", "Volcano");
            }
            else if (CavernEnd)
            {
                PlayerPrefs.SetInt("CavernComplete", 1);
                PlayerPrefs.SetString("MostRecentLevelComplete", "Cavern");
            }
            else if (SkyEnd)
            {
                PlayerPrefs.SetInt("StoneColdComplete", 1);
                PlayerPrefs.SetString("MostRecentLevelComplete", "StoneCold");
            }
            
            
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
