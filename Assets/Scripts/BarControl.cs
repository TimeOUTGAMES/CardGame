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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        economyBar.maxValue = maxBarValue;
        economyBar.minValue = minBarValue;
        economyBar.value = economyBar.maxValue / 2; // Ekonomi barýnýn baþlangýç deðeri

        farmBar.maxValue = maxBarValue;
        farmBar.minValue = minBarValue;
        farmBar.value = farmBar.maxValue / 2; // Tarým barýnýn baþlangýç deðeri

        publicBar.maxValue = maxBarValue;
        publicBar.minValue = minBarValue;
        publicBar.value = publicBar.maxValue / 2; // Halk barýnýn baþlangýç deðeri

        militaryBar.maxValue = maxBarValue;
        militaryBar.minValue = minBarValue;
        militaryBar.value = militaryBar.maxValue / 2; // Asker barýnýn baþlangýç deðeri

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

}
