using UnityEngine;
using UnityEngine.UI; // Обов'язково для роботи з UI

public class UIManager : MonoBehaviour
{
    [Header("UI Елементи")]
    public Slider healthBar; // Посилання на нашу смужку здоров'я

    // Ця функція налаштовує смужку на старті рівня
    public void SetupHealthBar(int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    // Ця функція оновлює смужку після удару
    public void UpdateHealthBar(int currentHealth)
    {
        healthBar.value = currentHealth;
    }
} 