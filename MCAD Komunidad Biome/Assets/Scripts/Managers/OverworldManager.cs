using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class OverworldManager : MonoBehaviour
{
    [Header("Coroutine Delays")]
    [SerializeField] private float moveDelay; // 0.5f
    [SerializeField] private float fadeDelay; // 0.5f

    [Header("Main Menu Scene")]
    [SerializeField] private string mainMenuScene; // MainMenuScene

    [Header("Scene Transition")]
    [SerializeField] private SceneTransition sceneTransition;

    [Header("Ending Cutscene Game Objects")]
    [SerializeField] private VideoPlayer cutsceneVideoPlayer;
    [SerializeField] private GameObject cutsceneGroup;
    [SerializeField] private GameObject cutsceneScreenGameObject;
    [SerializeField] private GameObject cutsceneSkipButtonGameObject;

    private Button continueButton;

    [Header("Sunset Controller")] // Reference: https://discussions.unity.com/t/how-edit-global-volume-profiles-from-script/858453/2
    [SerializeField] private SunSet settingSun;

    [Header("Player Controller")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private GameObject playerControlsGroup;

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

        // Set Up Ending Cutscene Game Objects
        continueButton = cutsceneSkipButtonGameObject.GetComponent<Button>();

        cutsceneVideoPlayer.loopPointReached += OnCutsceneFinished;
        cutsceneVideoPlayer.Stop();
        cutsceneVideoPlayer.Pause(); // Set Scene Cutscene
        cutsceneGroup.SetActive(false);

        /*
        HUDText.text = "";
        HUDGroup.SetActive(false);
        HUDTextGameObject.SetActive(false);

        cutsceneStart = false;
        cutsceneCounter = 0f;

        cutsceneIndex = 0;
        currentSubtitle = endingSubtitles.Subtitles[cutsceneIndex];
        */

        // Set Up Sunset Controller
        settingSun.PrepareSunset();

        // Set Up Player Controller
        AllowMovement();
    }

    void Update()
    {
        // Debug.Log("Counter: " + cutsceneCounter);
        /*
        if (cutsceneStart == true)
        {
            cutsceneCounter += Time.deltaTime;

            if (cutsceneCounter >= currentSubtitle.Time)
            {
                // Move to MainMenuScene when Narration Ends
                if (cutsceneCounter >= 112f)
                {
                    LoadMainMenuScene();

                    cutsceneStart = false;
                }

                HUDText.text = currentSubtitle.Subtitle;

                cutsceneIndex++;
                cutsceneIndex = Mathf.Clamp(cutsceneIndex, 0, endingSubtitles.Subtitles.Length - 1);
                currentSubtitle = endingSubtitles.Subtitles[cutsceneIndex];
            }
        }
        */
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

        SoundManager.instance.StopCurrentBGM();

        StartCoroutine(CO_TriggerCutscene());
    }

    IEnumerator CO_TriggerCutscene()
    {
        yield return new WaitForSeconds(fadeDelay + 1.0f);

        StartCutscene();
    }

    private void StartCutscene()
    {
        playerControlsGroup.SetActive(false);

        cutsceneGroup.SetActive(true);

        cutsceneVideoPlayer.Play();

        sceneTransition.StartFadeInTransition(fadeDelay + 0.5f);

        /*
        playerControlsGroup.SetActive(false);

        var playePrefab = Instantiate(playerCharacterPrefab, endingPlayerPosition.transform.position, Quaternion.identity);
        playePrefab.transform.SetParent(endingPlayerPosition.transform);

        sceneTransition.StartFadeInTransition(fadeDelay);

        SoundManager.instance.PlayNarration(0, 0.3f);

        cutsceneGroup.SetActive(true);
        cutsceneSkipButtonGameObject.SetActive(true);

        controller.gameObject.transform.position = endingCameraPosition.transform.position;
        controller.gameObject.transform.rotation = Quaternion.Euler(0f, -5f, 0f);

        settingSun.StartSunset();
        */
    }

    private void OnCutsceneFinished(VideoPlayer vp)
    {
        LoadMainMenuScene();
    }

    public void LoadMainMenuScene()
    {
        continueButton.enabled = false;

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
            playerControlsGroup.SetActive(false);
        }
    }

    public void ResumeFromDialogue()
    {
        if (controller != null)
        {
            controller.enabled = true;
            playerControlsGroup.SetActive(true);
        }
    }
}