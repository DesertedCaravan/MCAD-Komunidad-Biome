using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class MainManager : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private PlayerController controller;

    [Header("Cutscene")]
    [SerializeField] private GameObject cutsceneGroup;
    [SerializeField] private GameObject cutsceneSkipButtonGroup;
    [SerializeField] private Button cutsceneSkipButton;

    [Header("HUD")]
    [SerializeField] private GameObject HUDGroup; // only used at Start() to SetActive(true)
    [SerializeField] private GameObject HUDTextGroup;
    [SerializeField] private TextMeshProUGUI HUDText;

    public PlayerController Controller => controller;

    // Convert to Singleton
    public static MainManager instance = null; // public static means that it can be accessed

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

    void Start()
    {
        SoundManager.instance.InitializeTracks();
        PlayNarration(0);

        HUDGroup.SetActive(true);
        HUDTextGroup.SetActive(false);
    }

    public void PlayNarration(int narration)
    {
        controller.ToggleMovement(false);

        cutsceneGroup.SetActive(true);
        cutsceneSkipButtonGroup.SetActive(true);

        cutsceneSkipButton.onClick.RemoveAllListeners();

        switch (narration)
        {
            case 0:
                PlayStartingNarration();
                break;
            case 1:
                PlayEndingNarration();
                break;
            default:
                PlayStartingNarration();
                break;
        }
    }

    private void PlayStartingNarration()
    {
        SoundManager.instance.StopCurrentBGM();
        SoundManager.instance.StopCurrentNarration();
        SoundManager.instance.PlayNarration(0);

        cutsceneSkipButton.onClick.AddListener(StartGame);
    }

    private void PlayEndingNarration()
    {
        SoundManager.instance.StopCurrentBGM();
        SoundManager.instance.StopCurrentNarration();
        SoundManager.instance.PlayNarration(1);

        cutsceneSkipButton.onClick.AddListener(EndGame);
    }

    private void StartGame()
    {
        cutsceneGroup.SetActive(false);

        SoundManager.instance.StopCurrentBGM();
        SoundManager.instance.StopCurrentNarration();
        SoundManager.instance.PlayBGM(0);

        controller.ToggleMovement(true);
    }

    private void EndGame()
    {
        // cutsceneGroup.SetActive(false);

        // Exit to Menu Scene
    }

    public void PauseForDialogue()
    {
        if (controller != null)
        {
            controller.enabled = false;
        }
    }

    public void ResumeFromDialogue()
    {
        if (controller != null)
        {
            controller.enabled = true;
        }
    }
    public void DisplayHUD(string text, int timer)
    {
        HUDText.text = text;
        HUDTextGroup.SetActive(true);

        if (timer > 0)
        {
            StartCoroutine(CO_HideHUD(timer));
        }
    }

    IEnumerator CO_HideHUD(int timer)
    {
        yield return new WaitForSeconds(timer);

        HideHUD();
    }

    public void HideHUD()
    {
        HUDText.text = "";
        HUDTextGroup.SetActive(false);
    }
}
