using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BossData
{
    public string bossName = "Бос";
    public int maxHealth = 100;
    public int rewardCoins = 50; // Нагорода за перемогу над цим босом
    public Sprite normalPhoto;
    public Sprite damagedPhoto;
}

public class BossController : MonoBehaviour
{
    [Header("Список Усіх Босів")]
    public List<BossData> bosses = new List<BossData>();
    private int currentBossIndex = 0;
    private int currentHealth;

    [Header("Економіка")]
    public int currentCoins = 0;
    public int coinsPerClick = 1;

    [Header("Посилання")]
    public UIManager uiManager;
    public Image bossImageComponent;
    public GameObject damageTextPrefab;

    private bool isPhaseChanged = false;
    private Vector3 originalBossPosition;
    private Coroutine shakeCoroutine;

    void Start()
    {
        originalBossPosition = bossImageComponent.transform.localPosition;

        if (uiManager != null)
            uiManager.UpdateCoinsText(currentCoins);

        if (bosses.Count > 0)
        {
            LoadBoss(0);
        }
    }

    void LoadBoss(int index)
    {
        currentBossIndex = index;
        BossData boss = bosses[currentBossIndex];

        currentHealth = boss.maxHealth;
        isPhaseChanged = false;

        bossImageComponent.sprite = boss.normalPhoto;
        uiManager.SetupHealthBar(boss.maxHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentBossIndex >= bosses.Count) return;

        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;

        // Нараховуємо монети за кожен клік
        currentCoins += coinsPerClick;
        uiManager.UpdateCoinsText(currentCoins);

        uiManager.UpdateHealthBar(currentHealth);

        // Текст урону
        if (damageTextPrefab != null)
        {
            GameObject textObj = Instantiate(damageTextPrefab, transform.parent);
            Vector3 randomOffset = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0);
            textObj.transform.position = bossImageComponent.transform.position + randomOffset;
        }

        // Тряска
        if (bossImageComponent != null)
        {
            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
                bossImageComponent.transform.localPosition = originalBossPosition;
            }
            shakeCoroutine = StartCoroutine(ShakeBossImage(0.1f, 15f));
        }

        // Зміна фази при 50% HP
        BossData currentBoss = bosses[currentBossIndex];
        if (currentHealth <= currentBoss.maxHealth / 2 && !isPhaseChanged)
        {
            isPhaseChanged = true;
            bossImageComponent.sprite = currentBoss.damagedPhoto;
        }

        // Перевірка на смерть боса
        if (currentHealth <= 0)
        {
            // Бонусні монети за перемогу
            currentCoins += currentBoss.rewardCoins;
            uiManager.UpdateCoinsText(currentCoins);

            NextBoss();
        }
    }

    void NextBoss()
    {
        currentBossIndex++;
        if (currentBossIndex < bosses.Count)
        {
            LoadBoss(currentBossIndex);
        }
        else
        {
            Debug.Log("Всі боси переможені! Фінальна перемога!");
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