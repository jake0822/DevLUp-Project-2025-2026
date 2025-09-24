using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    public string targetSceneName;
    public TextMeshPro Text;

    private bool inCollider;

    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);   
        if (other.CompareTag("player"))
        {
            Text.enabled = true;
            inCollider = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
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
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
