using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // 基础属性
    public float baseAttackPower = 10f;
    public float baseAttackSpeed = 1f;
    public float baseMoveSpeed = 5f;
    public float baseWeaponSize = 1f;

    public float attackPowerMultiplier = 1f;
    public float attackSpeedMultiplier = 1f;
    public float moveSpeedMultiplier = 1f;
    public float weaponSizeMultiplier = 1f;

    public float maxHP = 100f;
    public float currentHP = 100f;
    public float scaleMultiplier = 1f;

    public bool enableTimeBasedBuff = false;
    float timeCounter = 0f;

    public PlayerBar bar;

    private bool isInvincible = false;

    // 新增属性
    [HideInInspector] public float damageTakenMultiplier = 1f;
    [HideInInspector] public float lifeStealPercent = 0f;
    [HideInInspector] public float healthRegenPerSecond = 0f;
    [HideInInspector] public bool gainExpOnHitDamage = false;
    [HideInInspector] public float extraDamageOnHitPercent = 0f;
    [HideInInspector] public float lifeRegenOnHitPercent = 0f;

    // 周围敌人检测参数
    public float enemyDetectRadius = 5f;
    public LayerMask enemyLayer;

    void Start()
    {
        currentHP = maxHP;
        UpdateHealthBar();
    }

    void Update()
    {
        if (enableTimeBasedBuff)
        {
            timeCounter += Time.deltaTime;
            attackPowerMultiplier += 0.001f * Time.deltaTime;
        }

        // 每秒恢复生命
        if (healthRegenPerSecond > 0f)
        {
            Heal(healthRegenPerSecond * Time.deltaTime);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isInvincible) return;

        // 乘以受伤倍率
        amount *= damageTakenMultiplier;

        currentHP -= amount;
        currentHP = Mathf.Max(0, currentHP);
        Debug.Log($"玩家受伤：-{amount} HP，当前HP: {currentHP}");
        UpdateHealthBar();

        if (gainExpOnHitDamage)
        {
            GainExp(1); // 受伤获得1点经验
        }

        StartCoroutine(InvincibilityCoroutine());

        if (currentHP <= 0)
        {
            Die();
        }
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(0.5f);
        isInvincible = false;
    }

    void UpdateHealthBar()
    {
        if (bar != null)
        {
            float percent = currentHP / maxHP;
            bar.SetHealth(percent);
        }
    }

    void Die()
    {
        Debug.Log("玩家死亡！");
        // 死亡逻辑
    }

    public void Heal(float amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        UpdateHealthBar();
    }

    public void HealPercent(float percent)
    {
        Heal(maxHP * percent);
    }

    public void GainExp(int amount)
    {
        Debug.Log($"获得经验 {amount}");
        // 你可以把经验逻辑放这里
    }

    /// <summary>
    /// 命中敌人后调用该方法
    /// </summary>
    public void OnHitEnemy(GameObject enemy, float damageDealt)
    {
        // 生命偷取
        if (lifeStealPercent > 0f)
        {
            float healAmount = damageDealt * lifeStealPercent;
            Heal(healAmount);
        }

        // 额外基于敌人当前生命的伤害
        if (extraDamageOnHitPercent > 0f)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject targetEnemy in enemies)
            {
                Monster monster = targetEnemy.GetComponent<Monster>();
                if (monster != null)
                {
                    float extraDmg = monster.maxHP * 0.15f;
                    monster.TakeDamage(Mathf.RoundToInt(extraDmg));
                }
            }


        }

        // 命中恢复生命
        if (lifeRegenOnHitPercent > 0f)
        {
            float healAmount = maxHP * lifeRegenOnHitPercent;
            Heal(healAmount);
        }
    }

    /// <summary>
    /// 获取周围敌人数量
    /// </summary>
    public int GetNearbyEnemyCount()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, enemyDetectRadius, enemyLayer);
        return hits.Length;
    }

    // 你还可以重写变更体型、武器尺寸等属性时的逻辑
    public void ApplyScale()
    {
        transform.localScale = Vector3.one * scaleMultiplier;
    }

    public void FlashRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            StopAllCoroutines(); // 避免叠加效果
            StartCoroutine(FlashRedPlayer(sr));
        }
    }

    private IEnumerator FlashRedPlayer(SpriteRenderer sr)
    {
        Color original = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(1f);
        sr.color = original;
    }

}
