using UnityEngine;

[System.Serializable]
public class DialogueBox
{
    [SerializeField] private bool isNPCSpeaking;
    [SerializeField] private string speaker;
    [SerializeField] [TextArea] private string dialogue;

    public bool IsNPCSpeaking => isNPCSpeaking;
    public string Speaker => speaker;
    public string Dialogue => dialogue;
}