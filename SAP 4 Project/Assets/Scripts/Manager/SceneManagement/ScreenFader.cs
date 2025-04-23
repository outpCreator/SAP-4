using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration = 1f;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator FadeToBlack(System.Action onComplete = null)
    {
        yield return Fade(1f, onComplete);
    }

    public IEnumerator FadeFromBlack(System.Action onComplete = null)
    {
        yield return Fade(0f, onComplete);
    }

    IEnumerator Fade(float targetAlpha, System.Action onComplete)
    {
        fadeImage.raycastTarget = true;

        float startAlpha = fadeImage.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0f, 0f, 0f, targetAlpha);
        fadeImage.raycastTarget = targetAlpha == 1f;

        onComplete?.Invoke();
    }
}
