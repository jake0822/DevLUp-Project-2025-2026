using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI Text;

    private bool inCollider;
    public GameObject DialogPanel;
    public DialogSystem ds;

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
            ds.canGoNext = false;
            ds.activeIndex = 0;
            DialogPanel.SetActive(false);
            
        }
    }

    public void startDialog(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DialogPanel.SetActive(true);
            ds.activeText.text = string.Empty;
            ds.nextDialog();
            ds.canGoNext = true;
            Text.enabled = false;
        }
    }
}
