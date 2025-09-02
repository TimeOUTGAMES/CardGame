using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class BarData
{
    public GameObject changeCircle;
    public Slider slider;
    public Image fillImage;
    public string barName;

    [HideInInspector] public float originalValue = -1f;
    [HideInInspector] public float originalFill = -1f;
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

    public float slightChangeScale = 0.2f;
    public float biggerChangeScale = 0.4f;
    public float tresholdValue = 20f;

    public bool isEndOfEra = false;

    public static BarControl Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitAllvars();
    }

    private void Update()
    {
        CheckBarValue();
    }

    public string CheckBarValue()
    {
        foreach (BarData bar in bars)
        {
            if (bar.slider.value <= 0)
            {
                isEndOfEra = true;
                return bar.barName;
            }
        }
        return "";
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

        float normalized = (bar.slider.value - minBarValue) / (maxBarValue - minBarValue);
        if (bar.fillImage != null)
            bar.fillImage.fillAmount = normalized;

        if (bar.changeCircle != null)
            bar.changeCircle.transform.localScale = Vector3.zero; // Başta gizli
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
        if (bar.slider == null || bar.changeCircle == null) return;

        if (change == 0f)
        {
            // Artık değişim yoksa çemberi gizle
            bar.changeCircle.transform.DOScale(0f, 0.2f);
            return;
        }

        // Değerleri sadece bir kez kaydet
        if (bar.originalValue == -1f)
            bar.originalValue = bar.slider.value;

        if (bar.originalFill == -1f && bar.fillImage != null)
            bar.originalFill = bar.fillImage.fillAmount;

        float scale = Mathf.Abs(change) > tresholdValue ? biggerChangeScale : slightChangeScale;
        bar.changeCircle.transform.DOScale(scale, 0.2f);
    }


    public void ApplyEffects(float economy, float farm, float publicVal, float military)
    {
        ApplySingleEffect(economyBar, economy);
        ApplySingleEffect(farmBar, farm);
        ApplySingleEffect(publicBar, publicVal);
        ApplySingleEffect(militaryBar, military);
    }

    private void ApplySingleEffect(BarData bar, float change)
    {
        if (bar.slider == null || bar.fillImage == null || bar.slider.value==0) return;
        
        float startValue = bar.slider.value;
        float endValue = Mathf.Clamp(startValue + change, minBarValue, maxBarValue);
        float duration = 0.5f;

        Color effectColor = Color.white;
        float intensity = Mathf.Min(Mathf.Abs(change) / 5f, 1f);

        if (change > 0)
            effectColor = Color.Lerp(Color.white, Color.green, intensity);
        else if (change < 0)
            effectColor = Color.Lerp(Color.white, Color.red, intensity);

        DOTween.To(() => bar.slider.value, x =>
        {
            bar.slider.value = x;
            float normalized = (x - minBarValue) / (maxBarValue - minBarValue);
            bar.fillImage.fillAmount = normalized;
        }, endValue, duration);

        float colorInTime = 0.4f;
        float colorHoldTime = 0.1f;
        float colorOutTime = 0.5f;

        bar.fillImage.DOColor(effectColor, colorInTime)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(colorHoldTime, () =>
                {
                    bar.fillImage.DOColor(Color.white, colorOutTime);
                });
            });

        if (bar.changeCircle != null)
            bar.changeCircle.transform.DOScale(0f, 0.2f);

        bar.originalValue = -1f;
        bar.originalFill = -1f;
        isEndOfEra = bar.slider.value <= 0;
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
            bar.slider.value = bar.originalValue;

        if (bar.fillImage != null && bar.originalFill != -1f)
        {
            bar.fillImage.DOFillAmount(bar.originalFill, 0.3f);
            bar.fillImage.DOColor(Color.white, 0.3f);
        }

        if (bar.changeCircle != null)
            bar.changeCircle.transform.DOScale(0f, 0.2f);

        bar.originalValue = -1f;
        bar.originalFill = -1f;
    }
}
