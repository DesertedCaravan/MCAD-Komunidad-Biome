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
    [SerializeField] private float fadeDelay; // 0.5f

    [Header("Main Menu Scene")]
    [SerializeField] private string mainMenuScene; // MainMenuScene

    [Header("Controller")]
    [SerializeField] private PlayerController controller;

    [Header("Volume")] // Reference: https://discussions.unity.com/t/how-edit-global-volume-profiles-from-script/858453/2
    // [SerializeField] private VolumeProfile volumeProfile;
    [SerializeField] private Material skybox;
    private bool _sunsetBool;
    private float _sunsetFloat;

    [Header("Scene Transition")]
    [SerializeField] private SceneTransition sceneTransition;
    [SerializeField] private GameObject cutsceneGroup;
    [SerializeField] private GameObject cutsceneSkipButtonGameObject;
    [SerializeField] private Button cutsceneSkipButton;
    [SerializeField] private GameObject HUDGroup; // only used at Start() to SetActive(true)
    [SerializeField] private GameObject HUDTextGameObject;
    [SerializeField] private TextMeshProUGUI HUDText;

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
        PlayNarration(0);

        HUDGroup.SetActive(true);
        HUDTextGameObject.SetActive(false);

        _sunsetBool = false;
        _sunsetFloat = 1.0f;
        SetSunset();
    }

    void Update()
    {
        if (_sunsetBool == true)
        {
            _sunsetFloat += 0.01f * Time.deltaTime;
            _sunsetFloat = Mathf.Clamp(_sunsetFloat, 0f, 2.0f);
            SetSunset();

            if (_sunsetFloat >= 2.0f)
            {
                _sunsetBool = false;
            }
        }
    }

    private void PlayNarration(int narration)
    {
        SceneFadeIn();

        controller.ToggleMovement(false);

        cutsceneGroup.SetActive(true);
        cutsceneSkipButtonGameObject.SetActive(true);

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

        cutsceneSkipButton.onClick.AddListener(ReturnToMainMenu);
    }

    private void StartGame()
    {
        cutsceneGroup.SetActive(false);

        SoundManager.instance.StopCurrentBGM();
        SoundManager.instance.StopCurrentNarration();
        SoundManager.instance.PlayBGM(0);

        controller.ToggleMovement(true);

        SetSunset();
        _sunsetBool = true;

        // WIP
        /*
        if (volumeProfile.TryGet(out SplitToning splitToning))
        {
            splitToning.active = true;
            splitToning.highlights.overrideState = true;

            // VolumeParameter volumeParameter = new Vector4Parameter(new Vector4(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
            // splitToning.highlights.SetValue(volumeParameter);
        }
        */
    }

    private void SetSunset()
    {
        // Reference: https://discussions.unity.com/t/how-can-i-change-the-atmosphere-thickness-skybox-material-with-another-script/878013/2
        skybox.SetFloat("_AtmosphereThickness", _sunsetFloat);
    }

    public void EnterCutscene(int cutscene)
    {
        controller.ToggleMovement(false); // Necessary Fix

        SceneFadeOut();

        StartCoroutine(CO_EnterCutscene(cutscene));
    }

    IEnumerator CO_EnterCutscene(int cutscene)
    {
        yield return new WaitForSeconds(fadeDelay + 1.0f);

        PlayNarration(cutscene);
    }

    private void ReturnToMainMenu()
    {
        SceneFadeOut();

        StartCoroutine(CO_ReturnToMainMenu());
    }

    IEnumerator CO_ReturnToMainMenu()
    {
        yield return new WaitForSeconds(fadeDelay + 1.0f);

        SceneManager.LoadScene(mainMenuScene);
    }

    public void SceneFadeIn()
    {
        sceneTransition.StartFadeInTransition(fadeDelay);
    }

    public void SceneFadeOut()
    {
        sceneTransition.StartFadeOutTransition(fadeDelay);
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