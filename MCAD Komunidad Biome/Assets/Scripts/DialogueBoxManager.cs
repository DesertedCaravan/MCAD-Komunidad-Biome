using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueBoxManager : MonoBehaviour
{
    [Header("Coroutine Delays")]
    [SerializeField] private float dialogueSkipDelay; // Default: 0.01f
    [SerializeField] private float dialogueEndDelay; // Default: 0.1f

    [Header("Key Binds")]
    [SerializeField] private Key interactKey; // SpaceKey

    [Header("Dialogue Box")]
    [SerializeField] private GameObject dialogueBoxGroup;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private BaseInteractable currentInteraction;
    private BaseTrigger currentTrigger;

    private DialogueText currentDialogueText;
    private string currentDialogueTextLine;
    private int textIndex; // Current index in currentDialogueText array

    [Header("Dialogue Response Events")]
    [SerializeField] private GameObject responseBox; // Response Box Panel
    [SerializeField] private GameObject responseButtonGameObject;
    private List<GameObject> tempResponseButtons = new List<GameObject>();
    private ResponseEvent[] responseEvents;

    [Header("Text Data")]
    private float textSpeed;
    [SerializeField] private float textDefault; // Default: 50f

    private float textDelay;
    [SerializeField] private float textDelayDefault; // Default: 0.05f

    private readonly List<Punctuation> punctuations = new List<Punctuation>()
    {
        new Punctuation(new HashSet<char>() {'.', '!', '?' }, 1.5f), // 0.6f
        new Punctuation(new HashSet<char>() {',', ';', ';' }, 1.0f), // 0.3f
    };

    private bool wait;
    private bool startDialogue;

    // Convert to Singleton
    public static DialogueBoxManager instance = null; // public static means that it can be accessed

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueBoxGroup.SetActive(false);
        dialogueBox.SetActive(false);
        startDialogue = false;

        responseBox.SetActive(false);

        textSpeed = textDefault;
        textDelay = textDelayDefault;
        wait = false;
    }

    // Update is called once per frame
    void Update() // While gameObject.SetActive(false) prevents the Update loop from occurring
    {
        if (startDialogue == true)
        {
            if ((Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current[interactKey].wasPressedThisFrame) && wait == false) // Left Click and Space Button // Formerly Left Click or E Button (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
            {
                if (dialogueText.text == currentDialogueText.Dialogue[textIndex])
                {
                    // Move to next text box
                    NextLine();
                }
                else
                {
                    // Skip the text loading and display the entire text box
                    StopAllCoroutines();
                    dialogueText.text = currentDialogueText.Dialogue[textIndex];
                    CheckForResponses();
                }
            }
        }
    }

    public void AddResponseEvents(ResponseEvent[] responses) // only called by CheckResponseEvents(DialogueText dialogueText) in BaseInteractable
    {
        responseEvents = responses;
    }

    public void TransitionToDialogue(BaseInteractable interaction, DialogueText dialogueText) // mainly called by CheckResponseEvents(DialogueText dialogueText) in BaseInteractable
    {
        dialogueBoxGroup.SetActive(true);

        currentInteraction = interaction;
        currentDialogueText = dialogueText; // dialogueText contains string[] dialogue & ResponseOptions[] responses

        textIndex = 0; // Starting text box
        wait = false;

        dialogueBox.gameObject.SetActive(true); // Make Dialogue Box visible

        StartCoroutine(CO_TypeLine());

        StartCoroutine(CO_PermitDialogueSkip());
    }

    public void TransitionToDialogueTrigger(BaseTrigger trigger, DialogueText dialogueText)
    {
        dialogueBoxGroup.SetActive(true);

        currentTrigger = trigger;
        currentDialogueText = dialogueText; // dialogueText contains string[] dialogue & ResponseOptions[] responses

        textIndex = 0; // Starting text box
        wait = false;

        dialogueBox.gameObject.SetActive(true); // Make Dialogue Box visible

        StartCoroutine(CO_TypeLine());

        StartCoroutine(CO_PermitDialogueSkip());
    }

    IEnumerator CO_PermitDialogueSkip()
    {
        yield return new WaitForSeconds(dialogueSkipDelay);

        startDialogue = true; // after CO_TypeLine to allow text to display normally and not accidentally overlap with void Update() function
    }

    void NextLine()
    {
        if (textIndex < currentDialogueText.Dialogue.Length - 1) // if there's more than one text box
        {
            textIndex++; // move to next index

            StartCoroutine(CO_TypeLine());
        }
        else // if there are no more text boxes left
        {
            StartCoroutine(CO_CloseDialogue());
        }
    }

    IEnumerator CO_TypeLine()
    {
        dialogueText.text = string.Empty;
        currentDialogueTextLine = null;

        if (textIndex > 0) // only delay transition after first text box
        {
            yield return new WaitForSeconds(textDelay);
        }

        // Current Typing Effect (higher textSpeed results in a faster text speed)

        currentDialogueTextLine = currentDialogueText.Dialogue[textIndex];

        float t = 0;
        int charIndex = 0;
        textSpeed = textDefault;

        while (charIndex < currentDialogueTextLine.Length && currentDialogueTextLine != null) // currentDialogueTextLine != null is a necessary fix (not really but still kept just in case)
        {
            int lastCharIndex = charIndex; // Depending on the speed, several characters might be typed at once during the same frame, so this variable acts as a starting point for each frame.

            t += Time.deltaTime * textSpeed;

            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, currentDialogueTextLine.Length);

            for (int i = lastCharIndex; i < charIndex; i++)
            {
                // check current item has punctuation
                // exclude last item in array
                // exclude next item in array

                bool isLast = i >= currentDialogueTextLine.Length - 1;

                dialogueText.text = currentDialogueTextLine.Substring(0, i + 1); // String is essentially viewing increasing portions of the current dialogue

                if (IsPunctuation(currentDialogueTextLine[i], out float waitTime) && !isLast && !IsPunctuation(currentDialogueTextLine[i + 1], out _))
                {
                    textSpeed = waitTime;
                }
                else
                {
                    textSpeed = textDefault;
                }

                // yield return new WaitForSeconds(textSpeed); // not needed due to textSpeed change affecting the variable t
            }

            yield return null;
        }

        dialogueText.text = currentDialogueTextLine; // String views the entire portion of the current dialogue

        CheckForResponses();
    }

    private bool IsPunctuation(char character, out float waitTime)
    {
        foreach (Punctuation punctuationCategory in punctuations) // Punctuation was formerly KeyValuePair<HashSet<char>, float>
        {
            if (punctuationCategory.Punctuations.Contains(character)) // Punctuations was formerly Key
            {
                waitTime = punctuationCategory.WaitTime; // Punctuations was formerly Value
                return true;
            }
        }

        waitTime = textDefault; // default
        return false;
    }

    public void CheckForResponses()
    {
        if (textIndex == currentDialogueText.Dialogue.Length - 1 && dialogueText.text == currentDialogueText.Dialogue[textIndex])
        {
            if (textIndex == currentDialogueText.Dialogue.Length - 1 && currentDialogueText.HasResponses)
            {
                ShowResponses(currentDialogueText.Responses);
            }
        }
    }

    public void ShowResponses(ResponseOptions[] options)
    {
        wait = true;

        for (int i = 0; i < options.Length; i++) // Old Version: foreach (ResponseOptions response in options)
        {
            ResponseOptions response = options[i];
            int responseIndex = i;

            GameObject responseButton = Instantiate(responseButtonGameObject.gameObject, responseBox.transform);
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response, responseIndex));

            DialogueOptionDisplay dialogueOption = responseButton.GetComponent<DialogueOptionDisplay>();
            dialogueOption.SetOptionText(response.OptionText);

            tempResponseButtons.Add(responseButton);
            responseButton.SetActive(true);
        }

        responseBox.SetActive(true);
    }

    private void OnPickedResponse(ResponseOptions response, int responseIndex)
    {
        responseBox.SetActive(false);

        foreach (GameObject button in tempResponseButtons) // remove all response buttons
        {
            Destroy(button);
        }

        tempResponseButtons.Clear();

        if (responseEvents != null && responseIndex <= responseEvents.Length) // check if responseIndex is within the in bounds of the ResponseEvents array
        {
            responseEvents[responseIndex].OnPickedResponse?.Invoke(); // check for a responseEvent in the chosen index, then invoke the OnPickedResponse UnityEvent if there is
        }

        responseEvents = null; // reset responseEvents array just in case

        if (response.OptionResult) // only show new dialogue if the response chosen has any dialogue to show
        {
            if (currentInteraction != null)
            {
                TransitionToDialogue(currentInteraction, response.OptionResult);

                currentInteraction.CheckResponseEvents(response.OptionResult); // check if the new dialogue text has any response events that are attached to the interacted Game Object
            }
            else if (currentTrigger != null)
            {
                TransitionToDialogueTrigger(currentTrigger, response.OptionResult);

                currentTrigger.CheckResponseEvents(response.OptionResult); // check if the new dialogue text has any response events that are attached to the interacted Game Object
            }
        }
        else
        {
            textDelay = 0;
            StartCoroutine(CO_CloseDialogue());
        }
    }

    IEnumerator CO_CloseDialogue()
    {
        yield return new WaitForSeconds(dialogueEndDelay);

        textDelay = textDelayDefault;

        if (currentInteraction != null)
        {
            currentInteraction.GetComponent<BaseInteractable>().AllowInteraction(); // Allow player to interact with object again.
            currentInteraction.GetComponent<BaseInteractable>().OnEndInteract(); // Runs code after dialogue is completed.
        }
        else if (currentTrigger != null)
        {
            currentTrigger.GetComponent<BaseTrigger>().OnEndDialogueInteract(); // Runs code after dialogue is completed.
        }

        currentInteraction = null;
        currentTrigger = null;

        currentDialogueText = null;
        currentDialogueTextLine = null;

        startDialogue = false;

        dialogueBox.gameObject.SetActive(false); // Make Dialogue Box not visible
        dialogueBoxGroup.SetActive(false);

        MainManager.instance.ResumeFromDialogue(); // Allow player to move again
    }

    private readonly struct Punctuation
    {
        public readonly HashSet<char> Punctuations;
        public readonly float WaitTime;

        public Punctuation(HashSet<char> punctutations, float waitTime)
        {
            Punctuations = punctutations;
            WaitTime = waitTime;
        }
    }
}