using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private string[] DialogLines;
    public TextMeshProUGUI activeText;
    public float textspeed = 0.1f;

    public bool canGoNext = false;
    public bool finishedDialogue = false;


    public int activeIndex = 0;
    public bool skip = false;

    private void Update()
    {
       
    }

    IEnumerator typeDialog(string text)
    {
        if (!Application.isPlaying) yield break;

        canGoNext = false;
        int i = 0;
        activeText.text = string.Empty;
        skip = false;
        while(i < text.Length)
        {
            activeText.text += text[i];
            i++;
            if (skip)
            {
                activeText.text = text;
                break;
            }
            yield return new WaitForSeconds(textspeed);
            
        }
        skip = false;
        canGoNext = true;
        activeIndex++;
    }
    
    public void nextDialog(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canGoNext && activeIndex < DialogLines.Length)
                StartCoroutine(typeDialog(DialogLines[activeIndex]));
            else if (activeIndex >= DialogLines.Length) {
                finishedDialogue = true;
            }
            else
            {
                print("No active Dialog");
                skip = true;

            }
        }
            
    }
}
