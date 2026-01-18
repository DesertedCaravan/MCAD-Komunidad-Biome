using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Coroutine Delays")]
    [SerializeField] private float fadeDelay; // 0.5f

    [Header("Scenes")]
    [SerializeField] private string gameScene; // OverworldScene

    [Header("Scene Transition")]
    [SerializeField] private SceneTransition sceneTransition;

    [Header("Menu Game Objects")]
    [SerializeField] private GameObject titleGameObject;
    [SerializeField] private GameObject startButtonGameObject;
    // [SerializeField] private GameObject settingsButtonGameObject;
    [SerializeField] private GameObject quitButtonGameObject;
    // [SerializeField] private GameObject settingsGroupGameObject;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private TextMeshProUGUI quitText;

    private Button startButton;
    // private Button settingsButton;
    private Button quitButton;

    private Image startImage;
    private Image quitImage;

    private bool menuFade;
    private float menuAlpha;

    [Header("Starting Cutscene Game Objects")]
    [SerializeField] private GameObject cutsceneGroup;
    [SerializeField] private GameObject cutsceneSkipButtonGameObject;
    [SerializeField] private Button cutsceneSkipButton;
    [SerializeField] private IslandRotation islandRotation;
    private Button continueButton;

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

        cutsceneGroup.SetActive(false);
        cutsceneSkipButtonGameObject.SetActive(false);

        startButton = startButtonGameObject.GetComponent<Button>();
        // settingsButton = settingsButtonGameObject.GetComponent<Button>();
        quitButton = quitButtonGameObject.GetComponent<Button>();
        continueButton = cutsceneSkipButtonGameObject.GetComponent<Button>();

        startImage = startButtonGameObject.GetComponent<Image>();
        quitImage = quitButtonGameObject.GetComponent<Image>();

        // settingsGroupGameObject.SetActive(false);

        menuFade = false;
        menuAlpha = 1.0f;
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
                menuFade = false;

                StartCutscene();
            }
        }
    }

    private Color SetColorAlpha(Color menuColor, float alpha)
    {
        return new Color(menuColor.r, menuColor.g, menuColor.b, alpha);
    }

    public void StartGame()
    {
        // Disable Buttons
        startButton.enabled = false;
        // settingsButton.enabled = false;
        quitButton.enabled = false;

        // Start Alpha Fade
        menuFade = true;
    }

    private void StartCutscene()
    {
        SoundManager.instance.PlayNarration(0);

        cutsceneGroup.SetActive(true);
        cutsceneSkipButtonGameObject.SetActive(true);

        // cutsceneSkipButton.onClick.RemoveAllListeners();
        // cutsceneSkipButton.onClick.AddListener(LoadOverworldScene);

        islandRotation.ZoomIn();
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
        // Disable Buttons
        startButton.enabled = false;
        // settingsButton.enabled = false;
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