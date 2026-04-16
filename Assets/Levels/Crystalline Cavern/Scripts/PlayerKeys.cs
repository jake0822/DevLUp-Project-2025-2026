using UnityEngine;
using UnityEngine.Events;

public class PlayerKeys : MonoBehaviour
{
    [SerializeField] private int totalKeys;
    [SerializeField] private UnityEvent collectAllEvent;

    private int keys = 0;

    public void AddKey()
    {
        keys++;
        if (keys == totalKeys)
        {
            collectAllEvent.Invoke();
        }
    }

    public void ResetKeys()
    {
        keys = 0;
    }
}
