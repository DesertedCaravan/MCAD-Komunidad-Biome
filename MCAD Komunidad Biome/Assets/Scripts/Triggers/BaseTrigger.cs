using UnityEngine;
using UnityEngine.Events;

public class BaseTrigger : MonoBehaviour
{
    public enum TypeStruct
    {
        DialogueBox,
        Popup
    }

    [Header("Interactable Data")]
    [SerializeField] private TypeStruct triggerType;
    [SerializeField] private DialogueText triggerDialogue;
    [SerializeField] [TextArea] private string _triggerPopup;
    private bool _wasTriggered;
    // private bool _wasExited;
    [SerializeField] private bool _allowRepeatTriggers;

    [Header("Response Events")]
    [SerializeField] private UnityEvent onStartResponse;
    [SerializeField] private UnityEvent onEndResponse;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && _wasTriggered == false)
        {
            if (triggerType == TypeStruct.DialogueBox)
            {
                CheckResponseEvents(triggerDialogue);

                OverworldManager.instance.PauseForDialogue(); // Keep Player in place and Stop Walking Animation
                DialogueBoxManager.instance.TransitionToDialogueTrigger(this, triggerDialogue);
            }

            OnStartInteract();

            if (_allowRepeatTriggers == false)
            {
                _wasTriggered = true;
            }
        }
    }

    public void OverrideRepeatTrigger() // In case allowRepeatTriggers is set to true, let other game objects toggle wasTriggered
    {
        _wasTriggered = true;
        // _wasExited = true;
    }

    public void OnStartInteract()
    {
        onStartResponse.Invoke();
    }

    public void OnEndDialogueInteract() // Is called in DialogueBoxManager so that both DialogueBox and Popup trigger the onEndResponse.Invoke() function
    {
        onEndResponse.Invoke();
    }

    public void CheckResponseEvents(DialogueText dialogueText) // Taken from BaseInteractable
    {
        // find DialogueEvent components attached to this Game Object and make sure that it matches
        foreach (DialogueEvent dialogueEvents in GetComponents<DialogueEvent>()) // Old Version: if(TryGetComponent(out DialogueEvent dialogueEvents))
        {
            if (dialogueEvents.DialogueText == dialogueText)
            {
                DialogueBoxManager.instance.AddResponseEvents(dialogueEvents.Events);
                break;
            }
        }
    }

    public void PlaySoundManagerTrack(int track)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayBGM(track);
        }
    }
}