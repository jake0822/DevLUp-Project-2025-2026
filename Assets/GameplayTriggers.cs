using UnityEngine;

public class GameplayTriggers : MonoBehaviour
{
    public LevelSwitcher[] levels;

    public void ActivatePortals()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].portalDeactivate = false;
            print(i);
        }
    }
}
