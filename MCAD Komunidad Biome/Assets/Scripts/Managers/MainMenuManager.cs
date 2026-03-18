using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuManager : MonoBehaviour
{
    [Header("Coroutine Delays")]
    [SerializeField] private float fadeDelay; // 0.5f

    [Header("Overworld Scene")]
    [SerializeField] private string gameScene; // OverworldScene

    [Header("Scene Transition")]
    [SerializeField] private SceneTransition sceneTransition;

    [Header("Menu Game Objects")]
    [SerializeField] private GameObject titleGameObject;
    [SerializeField] private GameObject startButtonGameObject;
    [SerializeField] private GameObject quitButtonGameObject;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private TextMeshProUGUI quitText;

    private Button startButton;
    private Button quitButton;

    private Image startImage;
    private Image quitImage;

    private bool menuFade;
    private float menuAlpha;

    [Header("Starting Cutscene Game Objects")]
    [SerializeField] private VideoPlayer cutsceneVideoPlayer;
    [SerializeField] private GameObject cutsceneGroup;
    [SerializeField] private GameObject cutsceneScreenGameObject;
    [SerializeField] private GameObject cutsceneSkipButtonGameObject;

    private Button continueButton;

    [Header("Island Controller")]
    [SerializeField] private IslandRotation islandRotation;

    // Convert to Singleton
    public static MainMenuManager instance = null; // public static means that it can be accessed

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

        // Set up Menu Game Objects
        startButton = startButtonGameObject.GetComponent<Button>();
        quitButton = quitButtonGameObject.GetComponent<Button>();

        startImage = startButtonGameObject.GetComponent<Image>();
        quitImage = quitButtonGameObject.GetComponent<Image>();

        menuFade = false;
        menuAlpha = 1.0f;

        // Set Up Starting Cutscene
        continueButton = cutsceneSkipButtonGameObject.GetComponent<Button>();

        cutsceneVideoPlayer.loopPointReached += OnCutsceneFinished;
        cutsceneVideoPlayer.Stop();
        cutsceneVideoPlayer.Pause(); // Set Scene Cutscene
        cutsceneGroup.SetActive(false);
    }

    void Update()
    {
        if (menuFade == true)
        {
            menuAlpha -= 0.75f * Time.deltaTime;

            titleText.color = SetColorAlpha(titleText.color, menuAlpha);
            startText.color = SetColorAlpha(startText.color, menuAlpha);
            quitText.color = SetColorAlpha(quitText.color, menuAlpha);

            startImage.color = SetColorAlpha(startImage.color, menuAlpha);
            quitImage.color = SetColorAlpha(quitImage.color, menuAlpha);

            if (menuAlpha <= 0f)
            {
                TriggerCutscene();

                menuFade = false;
            }
        }
    }

    private Color SetColorAlpha(Color menuColor, float alpha)
    {
        return new Color(menuColor.r, menuColor.g, menuColor.b, alpha);
    }

    public void StartGame()
    {
        startButton.enabled = false;
        quitButton.enabled = false;

        menuFade = true;
    }

    private void TriggerCutscene()
    {
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
        cutsceneGroup.SetActive(true);

        cutsceneVideoPlayer.Play();

        sceneTransition.StartFadeInTransition(fadeDelay);

        // islandRotation.ZoomIn();
    }

    private void OnCutsceneFinished(VideoPlayer vp)
    {
        LoadOverworldScene();
    }

    public void LoadOverworldScene()
    {
        continueButton.enabled = false;

        sceneTransition.StartFadeOutTransition(fadeDelay);

        StartCoroutine(CO_LoadNextScene());
    }

    IEnumerator CO_LoadNextScene()
    {
        yield return new WaitForSeconds(fadeDelay + 1.0f);

        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame()
    {
        startButton.enabled = false;
        quitButton.enabled = false;

        sceneTransition.StartFadeOutTransition(fadeDelay);

        StartCoroutine(CO_QuitGame());
    }

    IEnumerator CO_QuitGame()
    {
        yield return new WaitForSeconds(fadeDelay + 1.0f);

        Application.Quit();
    }
}