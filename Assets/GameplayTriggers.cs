using UnityEngine;

public class GameplayTriggers : MonoBehaviour
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
#if UNITY_EDITOR
    private static bool hasResetThisSession = false;
#endif

    void Awake()
    {
#if UNITY_EDITOR
        if (!hasResetThisSession)
        {
            Debug.Log("Clearing PlayerPrefs (once per play session)");
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            hasResetThisSession = true;
        }
#endif
    }
    void Start()
    {
        // Load saved state
        introFinished = PlayerPrefs.GetInt("IntroFinished", 0) == 1;

        volcanoComplete = PlayerPrefs.GetInt("VolcanoComplete", 0) == 1;
        cavernComplete = PlayerPrefs.GetInt("CavernComplete", 0) == 1;
        stoneColdComplete = PlayerPrefs.GetInt("StoneColdComplete", 0) == 1;

        // Apply everything
        ApplyLevelState();
        UpdateHubState();
    }

    void Update()
    {
        // FIRST TIME finishing intro dialogue
        if (!introFinished && dm.hasCompletedDialgue())
        {
            introFinished = true;
            PlayerPrefs.SetInt("IntroFinished", 1);

            ActivateAllPortals();
            ApplyLevelState();   // ensure consistency
            UpdateHubState();    // update dialogue
        }
    }

    // 🔓 Unlock all portals after intro
    void ActivateAllPortals()
    {
        if (volcano != null) volcano.portalDeactivate = false;
        if (cavern != null) cavern.portalDeactivate = false;
        if (stoneCold != null) stoneCold.portalDeactivate = false;
    }

    // 🧠 Apply saved completion state to visuals + portals
    void ApplyLevelState()
    {
        // Volcano
        if (volcanoComplete)
        {
            if (volcano != null) volcano.portalDeactivate = true;
            if (volcanoSprite != null) volcanoSprite.SetActive(true);
        }
        else
        {
            if (volcano != null) volcano.portalDeactivate = !introFinished;
        }

        // Cavern
        if (cavernComplete)
        {
            if (cavern != null) cavern.portalDeactivate = true;
            if (cavernSprite != null) cavernSprite.SetActive(true);
        }
        else
        {
            if (cavern != null) cavern.portalDeactivate = !introFinished;
        }

        // StoneCold
        if (stoneColdComplete)
        {
            if (stoneCold != null) stoneCold.portalDeactivate = true;
            if (stoneColdSprite != null) stoneColdSprite.SetActive(true);
        }
        else
        {
            if (stoneCold != null) stoneCold.portalDeactivate = !introFinished;
        }
    }

    // 🧠 Decide which dialogue plays
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
            // Intro dialogue handled elsewhere
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
