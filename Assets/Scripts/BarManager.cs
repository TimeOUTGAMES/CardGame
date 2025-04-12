using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    public Slider healthBar; // Sa�l�k bar�
    public Slider farmerBar; // �ift�i bar�
    public Slider peoplerBar; // Halk bar�
    public Slider EconomyBar; // Ekonomi bar�

    [Header("Bar Maksimum ve Minimum De�erleri")]
    public float maxBarValue = 100f;
    public float minBarValue = 0f;

    private void Start()
    {
        // Barlar� ba�lang�� de�erlerine ayarla
        healthBar.maxValue = maxBarValue;
        healthBar.minValue = minBarValue;
        healthBar.value = maxBarValue / 2; // Sa�l�k bar� ba�lang�� de�eri

        farmerBar.maxValue = maxBarValue;
        farmerBar.minValue = minBarValue;
        farmerBar.value = maxBarValue / 2; // �ift�i bar� ba�lang�� de�eri

        peoplerBar.maxValue = maxBarValue;
        peoplerBar.minValue = minBarValue;
        peoplerBar.value = maxBarValue / 2; // Halk bar� ba�lang�� de�eri

        EconomyBar.maxValue = maxBarValue;
        EconomyBar.minValue = minBarValue;
        EconomyBar.value = maxBarValue / 2; // Ekonomi bar� ba�lang�� de�eri
    }

    // Sa�l�k bar�n� art�r
    public void ModifyHealth(float value)
    {
        healthBar.value = Mathf.Clamp(healthBar.value + value, minBarValue, maxBarValue);
    }

    public void ModifyFarmer(float value)
    {
        farmerBar.value = Mathf.Clamp(farmerBar.value + value, minBarValue, maxBarValue);
    }

    public void ModifyPeople(float value)
    {
        peoplerBar.value = Mathf.Clamp(peoplerBar.value + value, minBarValue, maxBarValue);
    }

    public void ModifyEconomy(float value)
    {
        EconomyBar.value = Mathf.Clamp(EconomyBar.value + value, minBarValue, maxBarValue);
    }
}
