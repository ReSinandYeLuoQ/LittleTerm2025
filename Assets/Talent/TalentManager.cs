using System.Collections.Generic;
using UnityEngine;

public class TalentManager : MonoBehaviour
{
    public static TalentManager Instance;

    public List<int> ownedTalentIDs = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    public void LearnTalent(TalentData talent)
    {
        if (!ownedTalentIDs.Contains(talent.talentID))
        {
            ownedTalentIDs.Add(talent.talentID);
            talent.Activate(gameObject);
        }
    }

    public bool CanSelect(TalentData talent)
    {
        var tree = talent.tree;
        var tier = talent.tier;

        int tier1 = 0, tier2 = 0;

        foreach (int id in ownedTalentIDs)
        {
            TalentData owned = TalentDatabase.Instance.GetTalentByID(id);
            if (owned.tree != tree) continue;

            if (owned.tier == TalentTier.Tier1) tier1++;
            if (owned.tier == TalentTier.Tier2) tier2++;
        }

        if (tier == TalentTier.Tier1) return true;
        if (tier == TalentTier.Tier2) return tier1 >= 2;
        if (tier == TalentTier.Tier3) return tier2 >= 2;

        return false;
    }
}
