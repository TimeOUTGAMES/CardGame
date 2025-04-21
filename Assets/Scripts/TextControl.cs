using System.Collections;
using UnityEngine;

public class TextControl : MonoBehaviour
{
    public float fadeDuration = 1f;  // Fade süresi
    private CanvasGroup canvasGroup;

    void Start()
    {
        // Canvas Group bileþenini al
        canvasGroup = GetComponent<CanvasGroup>();

        // Baþlangýçta yazýyý görünmez yap
        canvasGroup.alpha = 0f;

        // Fade iþlemini baþlat
        StartCoroutine(FadeTextInAndOut());
    }

    // Fade iþlemini sürekli tekrarlamak için Coroutine
    IEnumerator FadeTextInAndOut()
    {
        while (true)
        {
            // Fade-in (görünür yap)
            yield return StartCoroutine(FadeText(0f, 1f));

            // Fade-out (kaybol)
            yield return StartCoroutine(FadeText(1f, 0f));
        }
    }

    // Alpha deðerini deðiþtirerek yazýyý görünür/kaybol yapma
    IEnumerator FadeText(float startAlpha, float endAlpha)
    {
        float time = 0;
        while (time < fadeDuration)
        {
            // CanvasGroup alpha deðerini zamanla deðiþtir
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration * 2);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = endAlpha;  // Son hali ayarla
    }
}
