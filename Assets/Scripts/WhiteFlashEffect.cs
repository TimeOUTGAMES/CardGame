using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WhiteFlashEffect : MonoBehaviour
{
    public Image whiteOverlay; // Canvas'taki beyaz Image
    public float fadeDuration = 5f;
    public static WhiteFlashEffect instance; // Singleton örneði

    public void Play()
    {
        whiteOverlay.color = new Color(1, 1, 1, 1); // Tam opak
        gameObject.SetActive(true);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2f); // Efektin baþlamasý için kýsa bir gecikme
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            whiteOverlay.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        whiteOverlay.color = new Color(1, 1, 1, 0);
        gameObject.SetActive(false); // Ýstersen efektten sonra kendini kapat
    }
}
