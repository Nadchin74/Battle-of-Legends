using UnityEngine;
using UnityEngine.UI; // Обов'язково для роботи з картинками UI

public class BossController : MonoBehaviour
{
    [Header("Характеристики Боса")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Посилання")]
    public UIManager uiManager;
    public Image bossImageComponent; // Посилання на компонент Image, який малює фото

    [Header("Фази Боса (Фотографії)")]
    public Sprite normalPhoto;  // Звичайне фото
    public Sprite damagedPhoto; // Смішне фото (менше 50% здоров'я)

    private bool isPhaseChanged = false; // Той самий прапорець оптимізації з GDD

    void Start()
    {
        currentHealth = maxHealth;
        uiManager.SetupHealthBar(maxHealth);
        bossImageComponent.sprite = normalPhoto; // На старті ставимо звичайне фото
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth < 0) currentHealth = 0;

        uiManager.UpdateHealthBar(currentHealth);

        // Перевірка на зміну фази (здоров'я <= 50% і фаза ще не змінювалася)
        if (currentHealth <= maxHealth / 2 && !isPhaseChanged)
        {
            isPhaseChanged = true; // Перемикаємо прапорець
            bossImageComponent.sprite = damagedPhoto; // Міняємо картинку
            Debug.Log("Фаза змінена! Бос отримав по обличчю!");
        }

        if (currentHealth == 0)
        {
            Debug.Log("Бос переможений! Перехід у магазин...");
        }
    }
} 