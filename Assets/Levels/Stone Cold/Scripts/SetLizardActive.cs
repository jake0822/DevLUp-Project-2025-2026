using UnityEngine;
using UnityEngine.Events;

public class SetLizardActive : MonoBehaviour
{
    public DialogManager dialogManager;
    public GameObject crawllizard;
    public GameObject floatlizard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        crawllizard.SetActive(false);
    }

    void Update() {
        if (dialogManager.hasCompletedDialgue()) {

            crawllizard.SetActive(true);
            floatlizard.SetActive(false);
        }
    }


}
