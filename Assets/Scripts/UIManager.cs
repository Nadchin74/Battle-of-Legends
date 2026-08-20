using UnityEngine;
using UnityEngine.UI;
using TMPro; // Для роботи з TextMeshPro

public class UIManager : MonoBehaviour
{
    public Slider healthSlider;
    public TextMeshProUGUI coinsText; // Посилання на текст монет

    public void SetupHealthBar(int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    public void UpdateHealthBar(int currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    // Нова функція оновлення лічильника монет
    public void UpdateCoinsText(int coins)
    {
        if (coinsText != null)
        {
            coinsText.text = $"🪙 {coins}";
        }
    }
} 