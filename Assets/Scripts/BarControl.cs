using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BarControl : MonoBehaviour
{
    public Slider economyBar;
    public Slider farmBar;
    public Slider publicBar;
    public Slider militaryBar;

    public float maxBarValue = 100f;
    public float minBarValue = 0f;

    public Image economyFill;
    public Image farmFill;
    public Image publicFill;
    public Image militaryFill;


    private float originalEconomyFill;
    private float originalFarmFill;
    private float originalPublicFill;
    private float originalMilitaryFill;

    private Coroutine economyCoroutine;
    private Coroutine farmCoroutine;
    private Coroutine publicCoroutine;
    private Coroutine militaryCoroutine;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // ... zaten var olan ayarların yanında
        originalEconomyFill = -1f;
        originalFarmFill = -1f;
        originalPublicFill = -1f;
        originalMilitaryFill = -1f;


        economyBar.maxValue = maxBarValue;
        economyBar.minValue = minBarValue;
        economyBar.value = economyBar.maxValue / 2; // Ekonomi barının başlangıç değeri

        farmBar.maxValue = maxBarValue;
        farmBar.minValue = minBarValue;
        farmBar.value = farmBar.maxValue / 2; // Tarım barının başlangıç değeri

        publicBar.maxValue = maxBarValue;
        publicBar.minValue = minBarValue;
        publicBar.value = publicBar.maxValue / 2; // Halk barının başlangıç değeri

        militaryBar.maxValue = maxBarValue;
        militaryBar.minValue = minBarValue;
        militaryBar.value = militaryBar.maxValue / 2; // Asker barının başlangıç değeri

    }


    public void ModifyEconomy(float value)
    {
        economyBar.value = Mathf.Clamp(economyBar.value + value, minBarValue, maxBarValue);
    }

    public void ModifyFarm(float value)
    {
        farmBar.value = Mathf.Clamp(farmBar.value + value, minBarValue, maxBarValue);
    }

    public void ModifyPublic(float value)
    {
        publicBar.value = Mathf.Clamp(publicBar.value + value, minBarValue, maxBarValue);
    }

    public void ModifyMilitary(float value)
    {
        militaryBar.value = Mathf.Clamp(militaryBar.value + value, minBarValue, maxBarValue);
    }

    public void PreviewBarEffects(float economy, float farm, float publicVal, float military)
    {
        SetBarPreview(economyBar, economyFill, economy, ref originalEconomyFill);
        SetBarPreview(farmBar, farmFill, farm, ref originalFarmFill);
        SetBarPreview(publicBar, publicFill, publicVal, ref originalPublicFill);
        SetBarPreview(militaryBar, militaryFill, military, ref originalMilitaryFill);
    }


    public void ResetBarColors()
    {
        ResetColor(economyFill, ref originalEconomyFill, ref economyCoroutine);
        if (economyCoroutine != null) StopCoroutine(economyCoroutine);

        ResetColor(farmFill, ref originalFarmFill, ref farmCoroutine);
        if (farmCoroutine != null) StopCoroutine(farmCoroutine);

        ResetColor(publicFill, ref originalPublicFill, ref publicCoroutine);
        if (publicCoroutine != null) StopCoroutine(publicCoroutine);

        ResetColor(militaryFill, ref originalMilitaryFill, ref militaryCoroutine);
        if (militaryCoroutine != null) StopCoroutine(militaryCoroutine);
    }



    private void SetBarPreview(Slider slider, Image fillImage, float value, ref float originalFill)
    {
        if (slider == null || fillImage == null) return;

        if (originalFill == -1f)
        {
            originalFill = fillImage.fillAmount;
        }

        float currentValue = slider.value;
        float previewValue = Mathf.Clamp(currentValue + value, minBarValue, maxBarValue);
        float normalizedPreviewValue = (previewValue - minBarValue) / (maxBarValue - minBarValue);

        float intensity = Mathf.Min(Mathf.Abs(value) / 5f, 1f);
        Color targetColor = Color.white;

        if (value > 0)
            targetColor = Color.Lerp(Color.white, Color.green, intensity);
        else if (value < 0)
            targetColor = Color.Lerp(Color.white, Color.red, intensity);

        // Sadece ilgili barın coroutine’ini iptal et
        Coroutine coroutineToStop = null;

        if (fillImage == economyFill) coroutineToStop = economyCoroutine;
        else if (fillImage == farmFill) coroutineToStop = farmCoroutine;
        else if (fillImage == publicFill) coroutineToStop = publicCoroutine;
        else if (fillImage == militaryFill) coroutineToStop = militaryCoroutine;

        if (coroutineToStop != null)
        {
            StopCoroutine(coroutineToStop);
        }

        Coroutine newCoroutine = StartCoroutine(AnimateBarPreview(fillImage, normalizedPreviewValue, targetColor));

        if (fillImage == economyFill) economyCoroutine = newCoroutine;
        else if (fillImage == farmFill) farmCoroutine = newCoroutine;
        else if (fillImage == publicFill) publicCoroutine = newCoroutine;
        else if (fillImage == militaryFill) militaryCoroutine = newCoroutine;
    }




    private void ResetColor(Image image, ref float originalFill, ref Coroutine coroutine)
    {
        if (image != null)
        {
            image.color = Color.white;
            if (originalFill != -1f)
            {
                image.fillAmount = originalFill; 
            }
            coroutine = StartCoroutine(AnimateBarReset(image, originalFill));
            originalFill = -1f; // Sıfırla ki bir sonraki dokunuşta tekrar kayıt yapılsın
        }
    }

    private IEnumerator AnimateBarReset(Image fillImage, float targetFill)
    {
        float duration = 0.3f;
        float time = 0f;
        float startFill = fillImage.fillAmount;
        Color startColor = fillImage.color;
        Color endColor = Color.white;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            fillImage.fillAmount = Mathf.Lerp(startFill, targetFill, t);
            fillImage.color = Color.Lerp(startColor, endColor, t);

            yield return null;
        }

        fillImage.fillAmount = targetFill;
        fillImage.color = endColor;
    }



    private IEnumerator AnimateBarPreview(Image fillImage, float targetFill, Color targetColor)
    {
        float duration = 0.3f; // Geçiş süresi
        float time = 0f;
        float startFill = fillImage.fillAmount;
        Color startColor = fillImage.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            fillImage.fillAmount = Mathf.Lerp(startFill, targetFill, t);
            fillImage.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        fillImage.fillAmount = targetFill;
        fillImage.color = targetColor;
    }



}
