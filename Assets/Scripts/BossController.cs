using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Характеристики Боса")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Посилання")]
    public UIManager uiManager;
    public Image bossImageComponent;
    public GameObject damageTextPrefab;

    [Header("Фази Боса (Фотографії)")]
    public Sprite normalPhoto;
    public Sprite damagedPhoto;

    private bool isPhaseChanged = false;
    private Vector3 originalBossPosition;
    private Coroutine shakeCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        uiManager.SetupHealthBar(maxHealth);
        bossImageComponent.sprite = normalPhoto;
        originalBossPosition = bossImageComponent.transform.localPosition;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;

        uiManager.UpdateHealthBar(currentHealth);

        // Створюємо текст урону прямо над босом із невеликим випадковим розкидом
        if (damageTextPrefab != null)
        {
            GameObject textObj = Instantiate(damageTextPrefab, transform.parent);

            // Випадковий зсув, щоб цифри не накладалися одна на одну
            Vector3 randomOffset = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0);
            textObj.transform.position = bossImageComponent.transform.position + randomOffset;
        }

        // Запускаємо тряску
        if (bossImageComponent != null)
        {
            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
                bossImageComponent.transform.localPosition = originalBossPosition;
            }
            shakeCoroutine = StartCoroutine(ShakeBossImage(0.1f, 15f));
        }

        // Перевірка фази
        if (currentHealth <= maxHealth / 2 && !isPhaseChanged)
        {
            isPhaseChanged = true;
            bossImageComponent.sprite = damagedPhoto;
        }
    }

    private IEnumerator ShakeBossImage(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            bossImageComponent.transform.localPosition = new Vector3(originalBossPosition.x + x, originalBossPosition.y + y, originalBossPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        bossImageComponent.transform.localPosition = originalBossPosition;
        shakeCoroutine = null;
    }
} 