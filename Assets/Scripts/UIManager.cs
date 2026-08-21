using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Панелі")]
    public GameObject mainMenuPanel;
    public GameObject gamePanel;
    public GameObject shopPanel;

    [Header("Елементи Геймплею")]
    public Slider healthSlider;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI upgradeButtonText;

    void Start()
    {
        // При запуску показуємо тільки Головне Меню
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (gamePanel != null) gamePanel.SetActive(false);
        if (shopPanel != null) shopPanel.SetActive(false);
    }

    public void StartGame()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gamePanel != null) gamePanel.SetActive(true);
    }

    public void ToggleShop(bool isOpen)
    {
        if (shopPanel != null) shopPanel.SetActive(isOpen);
    }

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
        if (healthSlider != null) healthSlider.value = currentHealth;
    }

    public void UpdateCoinsText(int coins)
    {
        if (coinsText != null) coinsText.text = $"🪙 {coins}";
    }

    public void UpdateUpgradeButtonUI(int cost, int currentDamage)
    {
        if (upgradeButtonText != null)
            upgradeButtonText.text = $"Урон +1 (Зараз: {currentDamage})\nЦіна: {cost} 🪙";
    }
} 