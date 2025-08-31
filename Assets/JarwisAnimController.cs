using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JarwisAnimController : MonoBehaviour
{
    [Header("Main Jarwis Movement")]
    [SerializeField] private Transform jarwisBody;
    [SerializeField] private float floatStrength = 0.2f;
    [SerializeField] private float floatDuration = 2f;

    [Header("Arms")]
    [SerializeField] private List<Transform> armJoints;
    [SerializeField] private float armRotateAmount = 5f;
    [SerializeField] private float armRotateDuration = 1.5f;
    [SerializeField] private float armRotateRandomOffset = 3f;

    [Header("Claw Pairs (e.g., left+right for a single hand)")]
    [SerializeField] private List<ClawPair> clawPairs;
    [SerializeField] private float clawRotateAmount = 20f;
    [SerializeField] private float clawSpeed = 0.3f;
    [SerializeField] private float clawIntervalMin = 1.5f;
    [SerializeField] private float clawIntervalMax = 3f;

    [Header("Fire Animation")]
    [SerializeField] private Transform fireTransform;
    [SerializeField] private float fireScaleAmount = 0.2f;
    [SerializeField] private float firePulseDuration = 0.5f;

    [System.Serializable]
    public class ClawPair
    {
        public Transform leftClaw;
        public Transform rightClaw;
    }

    void Start()
    {
        AnimateJarwisFloat();
        AnimateArms();
        AnimateClawsRandomly();
        AnimateFire();
    }

    void AnimateJarwisFloat()
    {
        if (jarwisBody == null) return;

        jarwisBody.DOLocalMoveY(jarwisBody.localPosition.y + floatStrength, floatDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    void AnimateArms()
    {
        foreach (Transform arm in armJoints)
        {
            float randomAmount = Random.Range(-armRotateAmount - armRotateRandomOffset, armRotateAmount + armRotateRandomOffset);
            float randomDuration = armRotateDuration + Random.Range(-0.5f, 0.5f);
            float randomDelay = Random.Range(0f, 0.5f);

            arm.DOLocalRotate(new Vector3(0, 0, randomAmount), randomDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .SetDelay(randomDelay);
        }
    }

    void AnimateClawsRandomly()
    {
        foreach (ClawPair pair in clawPairs)
        {
            if (pair.leftClaw != null)
                StartCoroutine(ClawWiggleRoutine(pair.leftClaw));

            if (pair.rightClaw != null)
                StartCoroutine(ClawWiggleRoutine(pair.rightClaw));
        }
    }

    IEnumerator ClawWiggleRoutine(Transform claw)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(clawIntervalMin, clawIntervalMax));

            float randomAngle = Random.Range(-clawRotateAmount, clawRotateAmount);
            float duration = Random.Range(clawSpeed * 0.8f, clawSpeed * 1.2f);

            claw.DOLocalRotate(new Vector3(0, 0, randomAngle), duration)
                .SetEase(Ease.InOutSine);
        }
    }

    void AnimateFire()
    {
        if (fireTransform == null) return;

        Vector3 originalScale = fireTransform.localScale;
        fireTransform.DOScale(originalScale + Vector3.one * fireScaleAmount, firePulseDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
