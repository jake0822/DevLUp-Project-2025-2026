using UnityEngine;

public class GameplayState : MonoBehaviour
{
    [Header("Portals")]
    public LevelSwitcher volcano;
    public LevelSwitcher cavern;
    public LevelSwitcher stoneCold;

    [Header("Sprites (activate when level complete)")]
    public GameObject volcanoSprite;
    public GameObject cavernSprite;
    public GameObject stoneColdSprite;

    [Header("Dialogue")]
    public DialogManager dm;
    public DialogSystem ds;

    public string[] midDialogue;
    public string[] completeDialogue;


    public AudioClip[] midAudio;
    public AudioClip[] endAudio;


    private bool introFinished = false;

    private bool volcanoComplete = false;
    private bool cavernComplete = false;
    private bool stoneColdComplete = false;

    void Start()
    {
        UpdateHubState();
    }

    void Update()
    {
        // FIRST TIME finishing intro dialogue
        if (!introFinished && dm.hasCompletedDialgue())
        {
            introFinished = true;
            ActivateAllPortals();
        }
    }

    // 🔓 Unlock everything
    void ActivateAllPortals()
    {
        volcano.portalDeactivate = false;
        cavern.portalDeactivate = false;
        stoneCold.portalDeactivate = false;
    }

    // 🏁 Call this when returning from a level
    public void OnLevelCompleted(string levelName)
    {
        switch (levelName)
        {
            case "Volcano":
                volcanoComplete = true;
                volcano.portalDeactivate = true;
                volcanoSprite.SetActive(true);
                break;

            case "Cavern":
                cavernComplete = true;
                cavern.portalDeactivate = true;
                cavernSprite.SetActive(true);
                break;

            case "StoneCold":
                stoneColdComplete = true;
                stoneCold.portalDeactivate = true;
                stoneColdSprite.SetActive(true);
                break;
        }

        UpdateHubState();
    }

    // 🧠 Decides what dialogue should be active
    void UpdateHubState()
    {
        if (AllLevelsComplete())
        {
            ds.SetDialog(completeDialogue, endAudio);
        }
        else if (AnyLevelComplete())
        {
            ds.SetDialog(midDialogue, midAudio);
        }
        else
        {
            //ds.SetDialog(introDialogue, introAudio);
        }
    }

    bool AllLevelsComplete()
    {
        return volcanoComplete && cavernComplete && stoneColdComplete;
    }

    bool AnyLevelComplete()
    {
        return volcanoComplete || cavernComplete || stoneColdComplete;
    }
}
