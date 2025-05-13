using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public CharacterStats playerStats;      // 인스펙터에서 연결
    public Slider healthSlider;             // 체력 표시용 슬라이더

    private void Start()
    {
        UpdateHealthBar();
    }

    private void Update()
    {
        if (playerStats != null)
        {
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        healthSlider.maxValue = playerStats.maxHealth;
        healthSlider.value = playerStats.currentHealth;
    }
}
