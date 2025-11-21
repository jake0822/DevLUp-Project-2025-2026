using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private string[] DialogLines;
    public TextMeshProUGUI activeText;
    public float textspeed = 0.1f;

    public bool test = false;
    private bool canGoNext = false;

    private void Update()
    {
        if (!Application.isPlaying) return;

        if (test)
        {
            test = false;
            StartCoroutine(typeDialog(DialogLines[0]));

        }
       
    }

    IEnumerator typeDialog(string text)
    {
        if (!Application.isPlaying) yield break;

        int i = 0;
        activeText.text = string.Empty;
        while(i < text.Length)
        {
            activeText.text += text[i];
            i++;
            yield return new WaitForSeconds(textspeed);
        }
    }
    public void nextDialog()
    {
        print("next Dialog");
    }
}
