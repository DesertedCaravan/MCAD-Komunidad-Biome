using System.Collections;
using UnityEngine;
using TMPro;

public class MainManager : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private PlayerController _controller;

    [Header("HUD")]
    [SerializeField] private GameObject HUDGroup; // only used at Start() to SetActive(true)
    [SerializeField] private GameObject HUDTextGroup;
    [SerializeField] private TextMeshProUGUI HUDText;

    public PlayerController Controller => _controller;

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
        HUDGroup.SetActive(true);
        HUDTextGroup.SetActive(false);
    }

    public void PauseForDialogue()
    {
        if (_controller != null)
        {
            _controller.enabled = false;
        }
    }

    public void ResumeFromDialogue()
    {
        if (_controller != null)
        {
            _controller.enabled = true;
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
