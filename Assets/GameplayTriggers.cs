using System.Collections.Generic;
using UnityEngine;


public class GameplayTriggers : MonoBehaviour
{
    private Dictionary<Renderer, Color[]> originalColors = new Dictionary<Renderer, Color[]>();
    private static readonly int ColorID = Shader.PropertyToID("_ColorMain_ScriptVar");
    [Header("Portals")]
    public LevelSwitcher volcano;
    public LevelSwitcher cavern;
    public LevelSwitcher stoneCold;

    [Header("PortalSprites")]
    public GameObject volcanoPortal;
    public GameObject cavernPortal;
    public GameObject stoneColdPortal;

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
    void CacheOriginalColors(GameObject portalVisuals)
    {
        if (portalVisuals == null) return;

        Renderer[] renderers = portalVisuals.GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            if (originalColors.ContainsKey(rend)) continue;

            Material[] mats = rend.materials;
            Color[] colors = new Color[mats.Length];

            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].HasProperty(ColorID))
                    colors[i] = mats[i].GetColor(ColorID);
                else
                    colors[i] = Color.white; // fallback
            }

            originalColors.Add(rend, colors);
        }
    }
    void dimThisPortal(GameObject portalVisuals)
    {
        if (portalVisuals == null) return;

        CacheOriginalColors(portalVisuals);

        Renderer[] renderers = portalVisuals.GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            Material[] mats = rend.materials;

            for (int i = 0; i < mats.Length; i++)
            {
                if (!mats[i].HasProperty(ColorID)) continue;

                Color original = mats[i].GetColor(ColorID);

                // Convert to gray but preserve brightness
                float grayValue = original.grayscale;
                Color gray = new Color(grayValue, grayValue, grayValue, original.a);

                mats[i].SetColor(ColorID, gray);
            }
        }
    }
    void UndimThisPortal(GameObject portalVisuals)
    {
        if (portalVisuals == null) return;

        CacheOriginalColors(portalVisuals);

        Renderer[] renderers = portalVisuals.GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            if (!originalColors.ContainsKey(rend)) continue;

            Material[] mats = rend.materials;
            Color[] savedColors = originalColors[rend];

            for (int i = 0; i < mats.Length; i++)
            {
                if (i < savedColors.Length && mats[i].HasProperty(ColorID))
                {
                    mats[i].SetColor(ColorID, savedColors[i]);
                }
            }
        }
    }
    void Start()
    {
        PlayerPrefs.DeleteAll();
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

            UndimThisPortal(volcanoPortal); // completed = always bright
        }
        else
        {
            if (volcano != null) volcano.portalDeactivate = !introFinished;

            if (introFinished)
                UndimThisPortal(volcanoPortal); // unlocked
            else
                dimThisPortal(volcanoPortal);   // locked at start
        }

        // Cavern
        if (cavernComplete)
        {
            if (cavern != null) cavern.portalDeactivate = true;
            if (cavernSprite != null) cavernSprite.SetActive(true);

            UndimThisPortal(cavernPortal);
        }
        else
        {
            if (cavern != null) cavern.portalDeactivate = !introFinished;

            if (introFinished)
                UndimThisPortal(cavernPortal);
            else
                dimThisPortal(cavernPortal);
        }

        // StoneCold
        if (stoneColdComplete)
        {
            if (stoneCold != null) stoneCold.portalDeactivate = true;
            if (stoneColdSprite != null) stoneColdSprite.SetActive(true);

            UndimThisPortal(stoneColdPortal);
        }
        else
        {
            if (stoneCold != null) stoneCold.portalDeactivate = !introFinished;

            if (introFinished)
                UndimThisPortal(stoneColdPortal);
            else
                dimThisPortal(stoneColdPortal);
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
