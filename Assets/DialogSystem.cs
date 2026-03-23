using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private string[] DialogLines;
    [SerializeField] private AudioClip[] DialogAudio;
    public AudioSource audio;

    public TextMeshProUGUI activeText;
    public float textspeed = 0.1f;

    public bool canGoNext = false;
    public bool finishedDialogue = false;


    public int activeIndex = 0;
    public bool skip = false;
    public bool autoPlay;
    private bool lockDialog;

    private void Update()
    {
       
    }

    IEnumerator typeDialog(string text)
    {
        if (!Application.isPlaying) yield break;

        
        audio.clip = DialogAudio[activeIndex];
        audio.Play();

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
        
        activeIndex++;
        
        if (autoPlay)
        {
            StartCoroutine(AutoPlay());
            canGoNext = false;
        }
        else
            canGoNext = true;
    }

    IEnumerator AutoPlay()
    {   canGoNext = false;
        yield return new WaitForSeconds(0.5f);
        lockDialog = true;
        nextDialog(new InputAction.CallbackContext());
        lockDialog = false;
    }
    
    public void nextDialog(InputAction.CallbackContext context)
    {
        if (context.performed || (autoPlay && lockDialog))
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
