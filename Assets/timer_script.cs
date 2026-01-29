using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class timer_script : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timerText.text = time.ToString();
    }

    public void resetText()
    {
        time = 0;
    }
}
