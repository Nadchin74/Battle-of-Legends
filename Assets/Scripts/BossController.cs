using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BossDataInfo
{
    public string bossName = "Бос";
    public int maxHealth = 100;
    public int rewardCoins = 50;
    public Sprite normalPhoto;
    public Sprite damagedPhoto;
}

public class BossController : MonoBehaviour
{
    [Header("Список Усіх Босів")]
    public List<BossDataInfo> bosses = new List<BossDataInfo>();
    private int currentBossIndex = 0;
    private int currentHealth;

    [Header("Економіка та Прокачка")]
    public int currentCoins = 0;
    public int damagePerClick = 1;
    public int upgradeCost = 10;

    [Header("Система Урону")]
    [Range(0f, 1f)] public float critChance = 0.15f;
    public float critMultiplier = 2.0f;

    [Header("Посилання")]
    public UIManager uiManager;
    public Image bossImageComponent;
    public GameObject damageTextPrefab;

    private bool isPhaseChanged = false;
    private Vector3 originalBossPosition;
    private Coroutine shakeCoroutine;

    void Start()
    {
        if (bossImageComponent != null) originalBossPosition = bossImageComponent.transform.localPosition;
        if (uiManager != null)
        {
            uiManager.UpdateCoinsText(currentCoins);
            uiManager.UpdateUpgradeButtonUI(upgradeCost, damagePerClick);
        }
        if (bosses != null && bosses.Count > 0) LoadBoss(0);
    }

    public void OnBossClicked()
    {
        int finalDamage = CalculateDamage();
        ReceiveDamage(finalDamage); // Викликаємо НОВУ назву функції
    }

    private int CalculateDamage()
    {
        float randomVariance = Random.Range(0.8f, 1.2f);
        float calculatedDamage = damagePerClick * randomVariance;
        if (Random.value <= critChance) calculatedDamage *= critMultiplier;
        return Mathf.Max(1, Mathf.RoundToInt(calculatedDamage));
    }

    // МИ ПЕРЕЙМЕНУВАЛИ ЦЮ ФУНКЦІЮ! Старі невидимі кнопки тепер зламаються.
    public void ReceiveDamage(int damageAmount)
    {
        if (bosses == null || currentBossIndex >= bosses.Count) return;

        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;

        currentCoins += 1;
        if (uiManager != null)
        {
            uiManager.UpdateCoinsText(currentCoins);
            uiManager.UpdateHealthBar(currentHealth);
        }

        SpawnDamageText(damageAmount);

        if (bossImageComponent != null)
        {
            if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
            shakeCoroutine = StartCoroutine(ShakeBossImage(0.1f, 15f));
        }

        BossDataInfo currentBoss = bosses[currentBossIndex];
        if (currentHealth <= currentBoss.maxHealth / 2 && !isPhaseChanged && bossImageComponent != null)
        {
            isPhaseChanged = true;
            if (currentBoss.damagedPhoto != null) bossImageComponent.sprite = currentBoss.damagedPhoto;
        }

        if (currentHealth <= 0)
        {
            currentCoins += currentBoss.rewardCoins;
            if (uiManager != null) uiManager.UpdateCoinsText(currentCoins);
            NextBoss();
        }
    }

    private void SpawnDamageText(int damageAmount)
    {
        if (damageTextPrefab != null && bossImageComponent != null)
        {
            GameObject textObj = Instantiate(damageTextPrefab, transform.parent);
            TMPro.TextMeshProUGUI tmp = textObj.GetComponent<TMPro.TextMeshProUGUI>();
            if (tmp != null)
            {
                if (damageAmount > damagePerClick * 1.5f) tmp.text = $"КРИТ! -{damageAmount}";
                else tmp.text = $"-{damageAmount}";
            }
            Vector3 randomOffset = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0);
            textObj.transform.position = bossImageComponent.transform.position + randomOffset;
        }
    }

    public void BuyDamageUpgrade()
    {
        if (currentCoins >= upgradeCost)
        {
            currentCoins -= upgradeCost;
            damagePerClick += 1;
            upgradeCost = Mathf.RoundToInt(upgradeCost * 1.5f);
            if (uiManager != null)
            {
                uiManager.UpdateCoinsText(currentCoins);
                uiManager.UpdateUpgradeButtonUI(upgradeCost, damagePerClick);
            }
        }
    }

    void LoadBoss(int index)
    {
        currentBossIndex = index;
        BossDataInfo boss = bosses[currentBossIndex];
        currentHealth = boss.maxHealth;
        isPhaseChanged = false;
        if (bossImageComponent != null && boss.normalPhoto != null) bossImageComponent.sprite = boss.normalPhoto;
        if (uiManager != null) uiManager.SetupHealthBar(boss.maxHealth);
    }

    void NextBoss()
    {
        currentBossIndex++;
        if (bosses != null && currentBossIndex < bosses.Count) LoadBoss(currentBossIndex);
    }

    private IEnumerator ShakeBossImage(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            if (bossImageComponent != null)
                bossImageComponent.transform.localPosition = new Vector3(originalBossPosition.x + x, originalBossPosition.y + y, originalBossPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (bossImageComponent != null) bossImageComponent.transform.localPosition = originalBossPosition;
        shakeCoroutine = null;
    }
} 