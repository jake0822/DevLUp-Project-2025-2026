using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisablePortals : MonoBehaviour
{
    public List<GameObject> portals;
    
    public void togglePortals()
    {
        for (int i = 0; i < portals.Count; i++)
        {
            gameObject.SetActive(gameObject.activeInHierarchy);
        }
    }
}
