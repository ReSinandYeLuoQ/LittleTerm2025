using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
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

    public bool enableTimeBasedBuff = false;
    float timeCounter = 0f;

    public PlayerBar bar;
    private bool isInvincible = false;
    void Start()
    {
        currentHP = maxHP;
        UpdateHealthBar();
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHP -= amount;
        currentHP = Mathf.Max(0, currentHP);
        Debug.Log($"玩家受伤：-{amount} HP，当前HP: {currentHP}");
        UpdateHealthBar();

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
            float percent = currentHP / (float)maxHP;
            bar.SetHealth(percent);
        }
    }

    void Die()
    {
        Debug.Log("玩家死亡！");
        // 死亡处理
    }

    void Update()
    {
        if (enableTimeBasedBuff)
        {
            timeCounter += Time.deltaTime;
            attackPowerMultiplier += 0.001f * Time.deltaTime;
        }
    }

    public void HealPercent(float percent)
    {
        float amount = maxHP * percent;
        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }

    // 可添加 GetXXX 方法方便武器访问真实属性
}
