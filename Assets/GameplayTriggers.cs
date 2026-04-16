using System.Collections.Generic;
using UnityEngine;


public class GameplayTriggers : MonoBehaviour
{
    
    [Header("Portals")]
    public LevelSwitcher volcano;
    public LevelSwitcher cavern;
    public LevelSwitcher stoneCold;

    [Header("Completion Sprites")]
    public GameObject volcanoSprite;
    public GameObject cavernSprite;
    public GameObject stoneColdSprite;

    [Header("Mother Dialogue")]
    public DialogSystem ds;
    public DialogManager dm;

    public string[] volcanoCompleteDialogue;
    public string[] cavernCompleteDialogue;
    public string[] stoneColdCompleteDialogue;
    public string[] completeDialogue;

    public AudioClip[] volcanoCompleteAudio;
    public AudioClip[] cavernCompleteAudio;
    public AudioClip[] stoneColdCompleteAudio;
    public AudioClip[] completeAudio;

    private bool introFinished;
    private bool volcanoComplete;
    private bool cavernComplete;
    private bool stoneColdComplete;
    private string MostRecentLevelComplete;

    void Start()
    {
        LoadState();
        ApplyHubState();
    }

    void Update()
    {
        // Detect first finished conversation with Mother
        if (!introFinished && dm != null && dm.hasCompletedDialgue())
        {
            FinishIntro();
        }
    }

    void LoadState()
    {
        MostRecentLevelComplete = PlayerPrefs.GetString("MostRecentLevelComplete", "");
        introFinished = PlayerPrefs.GetInt("IntroFinished", 0) == 1;
        volcanoComplete = PlayerPrefs.GetInt("VolcanoComplete", 0) == 1;
        cavernComplete = PlayerPrefs.GetInt("CavernComplete", 0) == 1;
        stoneColdComplete = PlayerPrefs.GetInt("StoneColdComplete", 0) == 1;
    }

    public void FinishIntro()
    {
        introFinished = true;
        PlayerPrefs.SetInt("IntroFinished", 1);

        ActivateAllPortals();
        ApplyHubState();
    }

    public void RefreshAfterReturningFromLevel()
    {
        LoadState();
        ApplyHubState();
    }

    void ApplyHubState()
    {
        UpdatePortals();
        UpdateSprites();
        UpdateDialogue();
    }

    void ActivateAllPortals()
    {
        SetPortalState(volcano, false);
        SetPortalState(cavern, false);
        SetPortalState(stoneCold, false);
    }

    void UpdatePortals()
    {
        if (!introFinished)
        {
            SetPortalState(volcano, true);
            SetPortalState(cavern, true);
            SetPortalState(stoneCold, true);
            return;
        }

        // Completed worlds become disabled when back in hub
        SetPortalState(volcano, volcanoComplete);
        SetPortalState(cavern, cavernComplete);
        SetPortalState(stoneCold, stoneColdComplete);
    }

    void UpdateSprites()
    {
        if (volcanoSprite != null)
            volcanoSprite.SetActive(volcanoComplete);

        if (cavernSprite != null)
            cavernSprite.SetActive(cavernComplete);

        if (stoneColdSprite != null)
            stoneColdSprite.SetActive(stoneColdComplete);
    }

    void UpdateDialogue()
    {
        if (!introFinished)
            return;

        dm.talkedOnce = true;

        if (AllLevelsComplete())
        {
            ds.SetDialog(completeDialogue, completeAudio);
        }
        else if (MostRecentLevelComplete == "Volcano")
        {
            ds.SetDialog(volcanoCompleteDialogue, volcanoCompleteAudio);
        }
        else if (MostRecentLevelComplete == "Cavern")
        {
            ds.SetDialog(cavernCompleteDialogue, cavernCompleteAudio);
        }
        else if (MostRecentLevelComplete == "StoneCold")
        {
            ds.SetDialog(stoneColdCompleteDialogue, stoneColdCompleteAudio);
        }

    }

    void SetPortalState(LevelSwitcher portal, bool deactivate)
    {
        if (portal != null)
            portal.portalDeactivate = deactivate;
    }

    bool AllLevelsComplete()
    {
        return volcanoComplete && cavernComplete && stoneColdComplete;
    }
}
