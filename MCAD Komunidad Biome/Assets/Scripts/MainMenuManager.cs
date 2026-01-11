using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Coroutine Delays")]
    [SerializeField] private float fadeDelay; // 0.5f

    [Header("Scenes")]
    [SerializeField] private string gameScene; // OverworldScene

    [Header("Scene Transition")]
    [SerializeField] private SceneTransition sceneTransition;
    [SerializeField] private GameObject startButtonGameObject;
    // [SerializeField] private GameObject settingsButtonGameObject;
    [SerializeField] private GameObject quitButtonGameObject;
    // [SerializeField] private GameObject settingsGroupGameObject;

    private Button startButton;
    // private Button settingsButton;
    private Button quitButton;

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

    public void Start()
    {
        startButton = startButtonGameObject.GetComponent<Button>();
        // settingsButton = settingsButtonGameObject.GetComponent<Button>();
        quitButton = quitButtonGameObject.GetComponent<Button>();

        // settingsGroupGameObject.SetActive(false);
    }

    public void StartGame()
    {
        // Disable Buttons
        startButton.enabled = false;
        // settingsButton.enabled = false;
        quitButton.enabled = false;

        sceneTransition.StartFadeOutTransition(fadeDelay);

        StartCoroutine(CO_StartGame());
    }

    IEnumerator CO_StartGame()
    {
        yield return new WaitForSeconds(fadeDelay + 1.0f);

        LoadNextScene();
    }

    public void LoadNextScene()
    {
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