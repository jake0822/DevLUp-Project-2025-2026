using UnityEngine;
using UnityEngine.Events;

public class PlayerKeys : MonoBehaviour
{
    [SerializeField] private int totalKeys;
    [SerializeField] private UnityEvent collectAllEvent;

    [SerializeField] private GameObject[] doorKeyObjects;

    private int keys = 0;

    public void AddKey()
    {
        keys++;
        UpdateDoorKeyObjects();

        if (keys == totalKeys)
        {
            collectAllEvent.Invoke();
        }
    }

    public void ResetKeys()
    {
        keys = 0;
        UpdateDoorKeyObjects();
    }

    public bool HasAllKeys()
    {
        return keys == totalKeys;
    }

    void UpdateDoorKeyObjects()
    {
        for (int i = 0; i < totalKeys; i++)
        {
            bool active = i < keys;
            doorKeyObjects[i].SetActive(active);
        }
    }
}
