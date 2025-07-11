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
        Debug.Log($"激活天赋：{talentName}");

        PlayerStats stats = player.GetComponent<PlayerStats>();

        switch (talentID)
        {
            case 0: stats.attackPowerMultiplier += 0.15f; break; // 伤害提升
            case 1: stats.attackPowerMultiplier += 0.3f; stats.moveSpeedMultiplier -= 0.3f; break;
            case 2: stats.attackSpeedMultiplier += 0.05f; stats.attackPowerMultiplier += 0.05f; break;
            case 3: stats.HealPercent(0.5f); stats.attackPowerMultiplier += 0.5f; break;
            case 4: stats.attackPowerMultiplier += 0.15f; stats.attackSpeedMultiplier += 0.1f; stats.enableTimeBasedBuff = true; break;
            case 5: stats.attackSpeedMultiplier -= 0.5f; stats.weaponSizeMultiplier += 3f; stats.attackPowerMultiplier += 2.25f; break;
                // 更多天赋后续添加
        }
    }
}
