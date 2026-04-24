using TMPro;
using UnityEngine;

public class KeysUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keysLabel;
    [SerializeField] private PlayerKeys playerKeys;

    void FixedUpdate()
    {
        keysLabel.text = $"Keys: {playerKeys.GetKeys()}/{playerKeys.GetTotalKeys()}";
    }
}
