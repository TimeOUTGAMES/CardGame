using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EyeController : MonoBehaviour
{
    public Transform eyeTransform;

    [Header("Blink Settings")]
    public float blinkMinDelay = 2f;
    public float blinkMaxDelay = 5f;
    public float blinkDuration = 0.1f;

    [Header("Look Settings")]
    public float lookAmount = 0.05f;
    public float lookInterval = 3f;

    private Vector3 originalLocalPos;
    private Vector3 originalScale;

    void Start()
    {
        if (eyeTransform == null) eyeTransform = transform;

        originalLocalPos = eyeTransform.localPosition;
        originalScale = eyeTransform.localScale;

        StartCoroutine(BlinkRoutine());
        StartCoroutine(LookAroundRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            float delay = Random.Range(blinkMinDelay, blinkMaxDelay);
            yield return new WaitForSeconds(delay);

            // Eye blink (Y scale küçülüp geri açılır)
            eyeTransform.DOScaleY(0.1f, blinkDuration).OnComplete(() =>
            {
                eyeTransform.DOScaleY(originalScale.y, blinkDuration);
            });
        }
    }

    IEnumerator LookAroundRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(lookInterval);

            // Sağ, sol veya ortayı rastgele seç
            float dir = Random.Range(-1f, 1f);
            Vector3 newPos = originalLocalPos + new Vector3(dir * lookAmount, 0f, 0f);

            eyeTransform.DOLocalMove(newPos, 0.3f).SetEase(Ease.InOutSine);
        }
    }
}