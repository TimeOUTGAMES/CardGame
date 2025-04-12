using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    public Slider healthBar; // Saðlýk barý
    public Slider farmerBar; // Çiftçi barý
    public Slider peoplerBar; // Halk barý
    public Slider EconomyBar; // Ekonomi barý

    [Header("Bar Maksimum ve Minimum Deðerleri")]
    public float maxBarValue = 100f;
    public float minBarValue = 0f;

    private void Start()
    {
        // Barlarý baþlangýç deðerlerine ayarla
        healthBar.maxValue = maxBarValue;
        healthBar.minValue = minBarValue;
        healthBar.value = maxBarValue / 2; // Saðlýk barý baþlangýç deðeri

        farmerBar.maxValue = maxBarValue;
        farmerBar.minValue = minBarValue;
        farmerBar.value = maxBarValue / 2; // Çiftçi barý baþlangýç deðeri

        peoplerBar.maxValue = maxBarValue;
        peoplerBar.minValue = minBarValue;
        peoplerBar.value = maxBarValue / 2; // Halk barý baþlangýç deðeri

        EconomyBar.maxValue = maxBarValue;
        EconomyBar.minValue = minBarValue;
        EconomyBar.value = maxBarValue / 2; // Ekonomi barý baþlangýç deðeri
    }

    // Saðlýk barýný artýr
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
