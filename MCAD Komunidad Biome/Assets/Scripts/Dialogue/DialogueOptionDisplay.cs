using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueOptionDisplay : MonoBehaviour
{
    [SerializeField] private Button optionButton;
    [SerializeField] private TextMeshProUGUI optionTextDisplay;

    public Button Button => optionButton;

    public void SetOptionText(string optionText)
    {
        optionTextDisplay.text = optionText;
    }
}