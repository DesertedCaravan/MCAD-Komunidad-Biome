using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    // Reference: https://www.bing.com/videos/riverview/relatedvideo?q=unity+fade+to+black+transition&mid=B8859B75566952AB1613B8859B75566952AB1613&FORM=VIRE

    [Header("Scene Transition")]
    [SerializeField] private Image sceneTransitionImage;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    public void StartFadeInTransition(float transitionDuration)
    {
        StartCoroutine(CO_FadeCoroutine(endColor, startColor, transitionDuration));
    }

    public void StartFadeOutTransition(float transitionDuration)
    {
        StartCoroutine(CO_FadeCoroutine(startColor, endColor, transitionDuration));
    }

    IEnumerator CO_FadeCoroutine(Color start, Color end, float transitionDuration)
    {
        float elapsedTime = 0;
        float elapsedPercentage = 0;

        while (elapsedPercentage < 1)
        {
            elapsedPercentage = elapsedTime / transitionDuration;
            sceneTransitionImage.color = Color.Lerp(start, end, elapsedPercentage);

            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }
}