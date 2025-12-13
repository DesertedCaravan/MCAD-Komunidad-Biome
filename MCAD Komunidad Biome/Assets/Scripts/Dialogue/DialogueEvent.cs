using UnityEngine;

public class DialogueEvent : MonoBehaviour
{
    [SerializeField] private DialogueText dialogueText;
    [SerializeField] private ResponseEvent[] events;

    public DialogueText DialogueText => dialogueText;
    public ResponseEvent[] Events => events;
}