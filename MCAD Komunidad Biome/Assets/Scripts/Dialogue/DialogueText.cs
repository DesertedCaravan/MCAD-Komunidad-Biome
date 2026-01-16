using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Text", menuName = "Dialogue Box/Dialogue Text")]
public class DialogueText : ScriptableObject
{
    [SerializeField] private DialogueBox[] dialogueBox;
    [SerializeField] private ResponseOptions[] responses;

    public DialogueBox[] DialogueBox => dialogueBox; // getter function
    public ResponseOptions[] Responses => responses;
    public bool HasResponses => Responses != null && Responses.Length > 0;
}