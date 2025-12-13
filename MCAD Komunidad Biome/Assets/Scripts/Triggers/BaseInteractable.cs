using UnityEngine;
using UnityEngine.Events;

public class BaseInteractable : MonoBehaviour, IInteractable
{
    [Header("Dialogue Text")]
    [SerializeField] protected DialogueText interactDialogue;
    [SerializeField] protected DialogueText interactedDialogue;

    [Header("Response Events")]
    [SerializeField] protected UnityEvent onStartResponse;
    [SerializeField] protected UnityEvent onLaterResponse;
    [SerializeField] protected UnityEvent onEndResponse;
    [SerializeField] [TextArea] private string responseText;
    [SerializeField] private int responseEndDelay;

    protected bool _interacting = false;
    protected bool _interactedCheck = false;

    public virtual void Interact()
    {
        if (!_interacting && _interactedCheck == false)
        {
            _interacting = true;
            _interactedCheck = true;

            CheckResponseEvents(interactDialogue);

            MainManager.instance.PauseForDialogue(); // Keep Player in place and Stop Walking Animation
            DialogueBoxManager.instance.TransitionToDialogue(this, interactDialogue);

            OnFirstInteract();
        }
        else if (!_interacting && _interactedCheck == true)
        {
            _interacting = true;

            CheckResponseEvents(interactedDialogue);

            MainManager.instance.PauseForDialogue(); // Keep Player in place and Stop Walking Animation
            DialogueBoxManager.instance.TransitionToDialogue(this, interactedDialogue);

            OnLaterInteract();
        }
    }

    public virtual void CheckResponseEvents(DialogueText dialogueText)
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

    // Note: Could add UnityEvents to the bottom three in the future
    protected virtual void OnFirstInteract()
    {
        onStartResponse.Invoke();
    }

    public virtual void OnEndInteract() // Occurs only after the dialogue has concluded
    {
        onEndResponse.Invoke();
    }

    protected virtual void OnLaterInteract()
    {
        onLaterResponse.Invoke();
    }

    public virtual void AllowInteraction()
    {
        _interacting = false;

        Debug.Log("INTERACTING RESET");
    }

    public virtual void ResetDialogue()
    {
        _interactedCheck = false;

        Debug.Log("DIALOGUE RESET");
    }
    public void PlayResponseText()
    {
        MainManager.instance.DisplayHUD(responseText, responseEndDelay);
    }
}