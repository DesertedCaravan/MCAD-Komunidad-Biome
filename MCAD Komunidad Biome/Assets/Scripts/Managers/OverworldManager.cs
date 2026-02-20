using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
// using UnityEngine.Rendering;
// using UnityEngine.Rendering.Universal;

public class OverworldManager : MonoBehaviour
{
    [Header("Coroutine Delays")]
    [SerializeField] private float moveDelay; // 0.5f
    [SerializeField] private float fadeDelay; // 0.5f

    [Header("Main Menu Scene")]
    [SerializeField] private string mainMenuScene; // MainMenuScene

    [Header("Controller")]
    [SerializeField] private PlayerController controller;

    [Header("Volume")] // Reference: https://discussions.unity.com/t/how-edit-global-volume-profiles-from-script/858453/2
    // [SerializeField] private VolumeProfile volumeProfile;
    [SerializeField] private SunSet settingSun;

    [Header("Scene Transition")]
    [SerializeField] private SceneTransition sceneTransition;
    [SerializeField] private GameObject cutsceneGroup;
    [SerializeField] private GameObject cutsceneSkipButtonGameObject;
    [SerializeField] private Button cutsceneSkipButton;
    [SerializeField] private GameObject HUDGroup; // only used at Start() to SetActive(true)
    [SerializeField] private GameObject HUDTextGameObject;
    [SerializeField] private TextMeshProUGUI HUDText;

    [Header("Player Character Prefab")]
    [SerializeField] private GameObject playerCharacterPrefab;
    [SerializeField] private GameObject endingCameraPosition;
    [SerializeField] private GameObject endingPlayerPosition;

    public PlayerController Controller => controller;

    // Convert to Singleton
    public static OverworldManager instance = null; // public static means that it can be accessed

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
        SoundManager.instance.PlayBGM(0);

        cutsceneGroup.SetActive(false);
        cutsceneSkipButtonGameObject.SetActive(false);

        HUDGroup.SetActive(true);
        HUDTextGameObject.SetActive(false);

        settingSun.PrepareSunset();

        AllowMovement();
    }

    public void AllowMovement()
    {
        StartCoroutine(CO_AllowMovement());
    }

    IEnumerator CO_AllowMovement()
    {
        yield return new WaitForSeconds(moveDelay);

        controller.ToggleMovement(true);
    }

    public void TriggerCutscene()
    {
        controller.ToggleMovement(false);

        sceneTransition.StartFadeOutTransition(fadeDelay);

        StartCoroutine(CO_TriggerCutscene());
    }

    IEnumerator CO_TriggerCutscene()
    {
        yield return new WaitForSeconds(fadeDelay + 1.0f);

        StartCutscene();
    }

    private void StartCutscene()
    {
        var playePrefab = Instantiate(playerCharacterPrefab, endingPlayerPosition.transform.position, Quaternion.identity);
        playePrefab.transform.SetParent(endingPlayerPosition.transform);

        sceneTransition.StartFadeInTransition(fadeDelay); // ADDED

        SoundManager.instance.PlayNarration(0, 0.3f);

        cutsceneGroup.SetActive(true);
        cutsceneSkipButtonGameObject.SetActive(true);

        controller.gameObject.transform.position = endingCameraPosition.transform.position;
        controller.gameObject.transform.rotation = Quaternion.Euler(0f, -5f, 0f);

        settingSun.StartSunset();

        // cutsceneSkipButton.onClick.RemoveAllListeners();
        // cutsceneSkipButton.onClick.AddListener(LoadMainMenuScene);
    }

    public void LoadMainMenuScene()
    {
        cutsceneSkipButton.enabled = false;

        sceneTransition.StartFadeOutTransition(fadeDelay);

        StartCoroutine(CO_LoadMainMenuScene());
    }

    IEnumerator CO_LoadMainMenuScene()
    {
        yield return new WaitForSeconds(fadeDelay + 1.0f);

        SceneManager.LoadScene(mainMenuScene);
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
        HUDTextGameObject.SetActive(true);

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
        HUDTextGameObject.SetActive(false);
    }
}