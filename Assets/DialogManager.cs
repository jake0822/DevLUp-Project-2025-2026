using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI Text;

    private bool inCollider;
    private bool inDialog;
    public GameObject DialogPanel;
    public DialogSystem ds;

    public AudioClip[] defaultClips;
    private int voiceLineIndex = 0;
    private bool startedDefaults;
    public AudioSource audio;

    public bool talkedOnce = false;

    
    private float clipTimer;
    private void Start()
    {
        clipTimer = 0f;
    }
    private void PlayDefaultVoiceLoop()
    {
        if (talkedOnce) return;
        if (defaultClips == null || defaultClips.Length == 0) return;

        // wait until timer allows next line
        if (audio.isPlaying) return;

        clipTimer -= Time.deltaTime;
        if (clipTimer > 0) return;

        AudioClip clip = defaultClips[voiceLineIndex];

        if (clip != null)
        {
            audio.clip = clip;
            audio.Play();

            clipTimer =  2f;
        }

        voiceLineIndex++;

        
        if (voiceLineIndex >= defaultClips.Length)
        {
            voiceLineIndex = 0;
        }
    }
    private void Update()
    {

        if (inDialog)
        {
            audio.Stop();
            return;
        }

        PlayDefaultVoiceLoop();
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        if (other.CompareTag("player") && !inDialog)
        {
            Text.enabled = true;
            inCollider = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            Text.enabled = false;
            inCollider = false;
            ds.canGoNext = false;
            ds.activeIndex = 0;
            DialogPanel.SetActive(false);
            inDialog = false;
            
            
        }
    }

    public void startDialog(InputAction.CallbackContext context)
    {
        if (context.performed && Time.timeScale != 0 && !inDialog && inCollider)
        {
            talkedOnce = true;
            audio.Stop();
            inDialog = true;
            DialogPanel.SetActive(true);
            ds.activeText.text = string.Empty;
           
            ds.canGoNext = true;
            Text.enabled = false;
            context = new InputAction.CallbackContext();
            ds.nextDialog(context);
        }
    }

    public bool hasCompletedDialgue() {
        if (ds.finishedDialogue) {
            return true;
        }
        return false;
    }
}
