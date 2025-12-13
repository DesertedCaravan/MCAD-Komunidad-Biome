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
    [SerializeField] [TextArea] private string triggerPopup;
    private bool wasTriggered;
    private bool wasExited;
    [SerializeField] private bool allowRepeatTriggers;

    [Header("Response Events")]
    [SerializeField] private UnityEvent onStartResponse;
    [SerializeField] private UnityEvent onEndResponse;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && wasTriggered == false)
        {
            if (triggerType == TypeStruct.DialogueBox)
            {
                CheckResponseEvents(triggerDialogue);

                MainManager.instance.PauseForDialogue(); // Keep Player in place and Stop Walking Animation
                DialogueBoxManager.instance.TransitionToDialogueTrigger(this, triggerDialogue);
            }
            else if (triggerType == TypeStruct.Popup)
            {
                MainManager.instance.DisplayHUD(triggerPopup, 0);
            }

            OnStartInteract();

            if (allowRepeatTriggers == false)
            {
                wasTriggered = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && wasExited == false)
        {
            if (triggerType == TypeStruct.Popup) // Note: TypeStruct.DialogueBox auto resolves by itself so there's no need to include it here
            {
                MainManager.instance.HideHUD();

                OnEndDialogueInteract();

                if (allowRepeatTriggers == false)
                {
                    wasExited = true;
                }
            }
        }
    }

    public void OverrideRepeatTrigger() // In case allowRepeatTriggers is set to true, let other game objects toggle wasTriggered
    {
        wasTriggered = true;
        wasExited = true;
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
            SoundManager.instance.PlayGameScreenTrack(track);
        }
    }
}