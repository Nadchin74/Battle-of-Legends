using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Це потрібно для таймерів (корутин)

public class BossController : MonoBehaviour
{
    [Header("Характеристики Боса")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Посилання")]
    public UIManager uiManager;
    public Image bossImageComponent;

    [Header("Фази Боса (Фотографії)")]
    public Sprite normalPhoto;
    public Sprite damagedPhoto;

    private bool isPhaseChanged = false;
    private Vector3 originalBossPosition; // Тут ми запам'ятаємо координати боса

    void Start()
    {
        currentHealth = maxHealth;
        uiManager.SetupHealthBar(maxHealth);
        bossImageComponent.sprite = normalPhoto;

        // Запам'ятовуємо нульову точку боса на старті
        originalBossPosition = bossImageComponent.transform.localPosition;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;

        uiManager.UpdateHealthBar(currentHealth);

        // Запускаємо тряску САМЕ БОСА (на 15 пікселів в сторони)
        StopAllCoroutines(); // Зупиняємо попередню тряску, якщо швидко клікаємо
        StartCoroutine(ShakeBossImage(0.1f, 15f));

        if (currentHealth <= maxHealth / 2 && !isPhaseChanged)
        {
            isPhaseChanged = true;
            bossImageComponent.sprite = damagedPhoto;
        }
    }

    // Та сама проста математична функція тряски з вашого GDD
    private IEnumerator ShakeBossImage(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Зсуваємо фотографію на випадкові пікселі
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            bossImageComponent.transform.localPosition = new Vector3(originalBossPosition.x + x, originalBossPosition.y + y, originalBossPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Жорстко повертаємо у вихідну нульову точку
        bossImageComponent.transform.localPosition = originalBossPosition;
    }
} 