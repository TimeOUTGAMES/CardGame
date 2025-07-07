using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BarData
{
    public Slider slider;
    public Image fillImage;
    public string barName;

    [HideInInspector] public float originalValue = -1f;
    [HideInInspector] public float originalFill = -1f;
    [HideInInspector] public Coroutine coroutine;
}

public class BarControl : MonoBehaviour
{
    
    public BarData economyBar;
    public BarData farmBar;
    public BarData publicBar;
    public BarData militaryBar;

    private List<BarData> bars;


    public float maxBarValue = 100f;
    public float minBarValue = 0f;

    public bool isEndOfEra = false;

    public static BarControl Instance;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        CheckBarValue();
    }

    public String CheckBarValue()
    {
        foreach(BarData barData in bars)
        {
            if (barData.slider.value <= 0)
            {
                isEndOfEra = true;
                return barData.barName;
            }
        }
        return "";
    }

    void Start()
    {
        InitAllvars();
    }

    public void InitAllvars()
    {
        InitBar(economyBar);
        InitBar(farmBar);
        InitBar(publicBar);
        InitBar(militaryBar);

        bars = new List<BarData> { economyBar, farmBar, publicBar, militaryBar };
    }

    private void InitBar(BarData bar)
    {
        if (bar.slider == null) return;
        bar.slider.maxValue = maxBarValue;
        bar.slider.minValue = minBarValue;
        bar.slider.value = (maxBarValue + minBarValue) / 2f;
    }

    public void PreviewBarEffects(float economy, float farm, float publicVal, float military)
    {
        PreviewSingleBar(economyBar, economy);
        PreviewSingleBar(farmBar, farm);
        PreviewSingleBar(publicBar, publicVal);
        PreviewSingleBar(militaryBar, military);
    }

    private void PreviewSingleBar(BarData bar, float change)
    {
        if (bar.slider == null || bar.fillImage == null) return;

        if (bar.originalValue == -1f)
            bar.originalValue = bar.slider.value;

        if (bar.originalFill == -1f)
            bar.originalFill = bar.fillImage.fillAmount;

        float previewValue = Mathf.Clamp(bar.originalValue + change, minBarValue, maxBarValue);
        float normalizedPreview = (previewValue - minBarValue) / (maxBarValue - minBarValue);

        Color targetColor = Color.white;
        float intensity = Mathf.Min(Mathf.Abs(change) / 5f, 1f);

        if (change > 0)
            targetColor = Color.Lerp(Color.white, Color.green, intensity);
        else if (change < 0)
            targetColor = Color.Lerp(Color.white, Color.red, intensity);

        if (bar.coroutine != null)
            StopCoroutine(bar.coroutine);

        bar.coroutine = StartCoroutine(AnimateBarPreview(bar, normalizedPreview, targetColor));

        bar.slider.value = previewValue;
    }

    public void ResetBarColors()
    {
        ResetSingleBar(economyBar);
        ResetSingleBar(farmBar);
        ResetSingleBar(publicBar);
        ResetSingleBar(militaryBar);
    }

    private void ResetSingleBar(BarData bar)
    {
        if (bar.slider != null && bar.originalValue != -1f)
        {
            bar.slider.value = bar.originalValue;
            // originalValue = -1f kaldırıldı, coroutine sonunda sıfırlanacak
        }

        if (bar.fillImage != null && bar.originalFill != -1f)
        {
            if (bar.coroutine != null)
                StopCoroutine(bar.coroutine);

            bar.coroutine = StartCoroutine(AnimateBarReset(bar));
        }
    }


    public void ApplyEffects(float economy, float farm, float publicVal, float military)
    {
        ApplySingleEffect(economyBar, economy);
        ApplySingleEffect(farmBar, farm);
        ApplySingleEffect(publicBar, publicVal);
        ApplySingleEffect(militaryBar, military);
    }

    private void ApplySingleEffect(BarData bar, float value)
    {
        if (bar.slider == null) return;
        bar.slider.value = Mathf.Clamp(bar.slider.value + value, minBarValue, maxBarValue);
    }

    private IEnumerator AnimateBarPreview(BarData bar, float targetFill, Color targetColor)
    {
        float duration = 0.3f;
        float time = 0f;
        float startFill = bar.fillImage.fillAmount;
        Color startColor = bar.fillImage.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            bar.fillImage.fillAmount = Mathf.Lerp(startFill, targetFill, t);
            bar.fillImage.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        bar.fillImage.fillAmount = targetFill;
        bar.fillImage.color = targetColor;
    }

    private IEnumerator AnimateBarReset(BarData bar)
    {
        float duration = 0.3f;
        float time = 0f;
        float startFill = bar.fillImage.fillAmount;
        Color startColor = bar.fillImage.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            bar.fillImage.fillAmount = Mathf.Lerp(startFill, bar.originalFill, t);
            bar.fillImage.color = Color.Lerp(startColor, Color.white, t);

            yield return null;
        }

        // Final değerler
        bar.fillImage.fillAmount = bar.originalFill;
        bar.fillImage.color = Color.white;

        // 🛠 Burada sıfırlama yapılır:
        bar.originalFill = -1f;
        bar.originalValue = -1f;
    }

}
