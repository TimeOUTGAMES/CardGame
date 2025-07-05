using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WhiteFlashEffect : MonoBehaviour
{
    public Image whiteOverlay;
    public float fadeDuration = 5f;
    public static WhiteFlashEffect Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Play()
    {
        // Baþlangýçta tamamen beyaz görünür yap
        whiteOverlay.color = new Color(1, 1, 1, 1);
        gameObject.SetActive(true);

        GameManager.instance.cardsTransform.gameObject.SetActive(false);
        GameManager.instance.bgCard.SetActive(false);

        // .5f saniye bekle ve sonra fade baþlat
        DOVirtual.DelayedCall(0.5f, () =>
        {
            whiteOverlay.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                // Fade tamamlandýktan sonra 1 saniye bekle ve sonra kapat
                DOVirtual.DelayedCall(1f, () =>
                {
                    whiteOverlay.color = new Color(1, 1, 1, 0);
                    gameObject.SetActive(false);
                });
            });
        });
    }
}
