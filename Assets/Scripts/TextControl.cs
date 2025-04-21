using System.Collections;
using UnityEngine;

public class TextControl : MonoBehaviour
{
    public float fadeDuration = 1f;  // Fade s�resi
    private CanvasGroup canvasGroup;

    void Start()
    {
        // Canvas Group bile�enini al
        canvasGroup = GetComponent<CanvasGroup>();

        // Ba�lang��ta yaz�y� g�r�nmez yap
        canvasGroup.alpha = 0f;

        // Fade i�lemini ba�lat
        StartCoroutine(FadeTextInAndOut());
    }

    // Fade i�lemini s�rekli tekrarlamak i�in Coroutine
    IEnumerator FadeTextInAndOut()
    {
        while (true)
        {
            // Fade-in (g�r�n�r yap)
            yield return StartCoroutine(FadeText(0f, 1f));

            // Fade-out (kaybol)
            yield return StartCoroutine(FadeText(1f, 0f));
        }
    }

    // Alpha de�erini de�i�tirerek yaz�y� g�r�n�r/kaybol yapma
    IEnumerator FadeText(float startAlpha, float endAlpha)
    {
        float time = 0;
        while (time < fadeDuration)
        {
            // CanvasGroup alpha de�erini zamanla de�i�tir
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration * 2);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = endAlpha;  // Son hali ayarla
    }
}
