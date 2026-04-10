using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class motion : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private TMP_Text m_TextComponent;
    float startingPos;
    private void Start()
    {
        startingPos = transform.position.y;
    }
    void Update()
    {
        if (transform.position.y >= (m_TextComponent.bounds.size.y) + 0 + startingPos)
        {
            //open main menu
        }
        else
        {
           transform.position = transform.position + (new Vector3(0, speed, 0));
        }
    }
}
