using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI Text;

    private bool inCollider;
    private bool inDialog;
    public GameObject DialogPanel;
    public DialogSystem ds;

    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        if (other.CompareTag("player") && !inDialog)
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
            ds.canGoNext = false;
            ds.activeIndex = 0;
            DialogPanel.SetActive(false);
            inDialog = false;
            
        }
    }

    public void startDialog(InputAction.CallbackContext context)
    {
        if (context.performed && Time.timeScale != 0 && !inDialog && inCollider)
        {
            inDialog = true;
            DialogPanel.SetActive(true);
            ds.activeText.text = string.Empty;
           
            ds.canGoNext = true;
            Text.enabled = false;
            context = new InputAction.CallbackContext();
            ds.nextDialog(context);
        }
    }
}
