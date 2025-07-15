using UnityEngine;

public enum TalentTreeID { TreeA, TreeB, TreeC, TreeD }
public enum TalentTier { Tier1, Tier2, Tier3 }

[CreateAssetMenu(fileName = "TalentData", menuName = "Talent/New Talent")]
public class TalentData : ScriptableObject
{
    public string talentName;
    public string description;
    public Sprite icon;

    public TalentTreeID tree;
    public TalentTier tier;
    public int talentID;

    public void Activate(GameObject player)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();
        if (stats == null) return;

        switch (talentID)
        {
            // 物理伤害树
            case 0: stats.attackPowerMultiplier += 0.15f; break;
            case 1: stats.attackPowerMultiplier += 0.3f; stats.moveSpeedMultiplier -= 0.3f; break;
            case 2: stats.attackSpeedMultiplier += 0.05f; stats.attackPowerMultiplier += 0.05f; break;
            case 3: stats.HealPercent(0.5f); stats.attackPowerMultiplier += 0.5f; break;
            case 4: stats.attackPowerMultiplier += 0.15f; stats.attackSpeedMultiplier += 0.1f; stats.enableTimeBasedBuff = true; break;
            case 5: stats.attackSpeedMultiplier -= 0.5f; stats.weaponSizeMultiplier += 3f; stats.attackPowerMultiplier += 2.25f; break;

            // 命中后树
            case 6: stats.damageTakenMultiplier *= 0.9f; break;
            case 7: stats.lifeStealPercent += 0.05f; break;
            case 8: stats.healthRegenPerSecond += stats.maxHP * 0.005f; break; // 0.5%每秒
            case 9: stats.damageTakenMultiplier *= 0.8f; break;
            case 10: stats.gainExpOnHitDamage = true; break;
            case 11: stats.damageTakenMultiplier *= 0.5f; stats.healthRegenPerSecond += stats.maxHP * 0.1f; break;

            // 攻击速度树
            case 12: stats.attackSpeedMultiplier += 0.3f; stats.attackPowerMultiplier -= 0.05f; break;
            case 13: stats.attackSpeedMultiplier += 0.1f; stats.moveSpeedMultiplier += 0.1f; break;
            case 14: stats.attackSpeedMultiplier += stats.moveSpeedMultiplier * 0.5f; break;
            case 15: stats.moveSpeedMultiplier += 0.6f; stats.extraDamageOnHitPercent += 0.15f; break;
            case 16: stats.attackPowerMultiplier -= 0.75f; stats.attackSpeedMultiplier += 3.0f; break;
            case 17: stats.lifeRegenOnHitPercent += 0.01f; stats.weaponSizeMultiplier -= 0.1f; break;

            // 移动速度树
            case 18: stats.moveSpeedMultiplier += 0.15f; break;
            case 19: stats.moveSpeedMultiplier += 0.05f; stats.maxHP *= 1.1f; stats.Heal(0); break;
            case 20: stats.moveSpeedMultiplier += 0.05f; stats.scaleMultiplier *= 0.8f; stats.ApplyScale(); break;
            case 21: stats.scaleMultiplier *= 0.8f; stats.ApplyScale(); break;
            case 22:
                int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
                stats.moveSpeedMultiplier += 0.05f * enemyCount;
                break;
            case 23: stats.scaleMultiplier *= 0.8f; stats.maxHP *= 1.5f; stats.Heal(0); stats.ApplyScale(); break;

            default:
                Debug.LogWarning($"未处理的天赋ID: {talentID}");
                break;
        }

        Debug.Log($"激活天赋：{talentName}");
    }
}
