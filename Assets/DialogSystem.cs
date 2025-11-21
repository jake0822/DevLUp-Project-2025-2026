using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private string[] DialogLines;
    public TextMeshProUGUI activeText;
    public float textspeed = 0.1f;

    
    public bool canGoNext = false;

    public int activeIndex = 0;

    private void Update()
    {
       
    }

    IEnumerator typeDialog(string text)
    {
        if (!Application.isPlaying) yield break;

        canGoNext = false;
        int i = 0;
        activeText.text = string.Empty;
        while(i < text.Length)
        {
            activeText.text += text[i];
            i++;
            yield return new WaitForSeconds(textspeed);
        }
        canGoNext = true;
        activeIndex++;
    }
    public void nextDialog()
    {
        if(canGoNext && activeIndex < DialogLines.Length)
            StartCoroutine(typeDialog(DialogLines[activeIndex]));
        else
        {
            print("No active Dialog");

        }
            
    }
}
