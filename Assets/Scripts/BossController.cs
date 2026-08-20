using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Характеристики Боса")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Посилання")]
    public UIManager uiManager; // Посилання на скрипт інтерфейсу

    void Start()
    {
        currentHealth = maxHealth; // На старті здоров'я повне
        uiManager.SetupHealthBar(maxHealth); // Передаємо дані в інтерфейс
    }

    // Ця функція викликається при кожному кліці по невидимій кнопці
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Віднімаємо здоров'я

        // Не даємо здоров'ю впасти нижче нуля
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        uiManager.UpdateHealthBar(currentHealth); // Оновлюємо смужку на екрані

        if (currentHealth == 0)
        {
            Debug.Log("Бос переможений! Перехід у магазин...");
        }
    }
} 