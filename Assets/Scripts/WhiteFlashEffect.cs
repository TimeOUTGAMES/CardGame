using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WhiteFlashEffect : MonoBehaviour
{
    public Image whiteOverlay; // Canvas'taki beyaz Image
    public float fadeDuration = 5f;
    public static WhiteFlashEffect instance; // Singleton �rne�i

    public void Play()
    {
        whiteOverlay.color = new Color(1, 1, 1, 1); // Tam opak
        gameObject.SetActive(true);
        //GameOpening.instance.enabled = true;
        GameManager.instance.cardsTransform.gameObject.SetActive(false);
        GameManager.instance.bgCard.SetActive(false);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(.5f);
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            whiteOverlay.color = new Color(1, 1, 1, alpha);
            
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        whiteOverlay.color = new Color(1, 1, 1, 0);
        gameObject.SetActive(false);    
        //GameOpening.instance.enabled = false;
    }

    private void Update()
    {
        GameOpening.instance.MoveCards();
    }
}
