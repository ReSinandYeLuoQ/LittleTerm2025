using System.Collections.Generic;
using UnityEngine;

public class TalentDatabase : MonoBehaviour
{
    public static TalentDatabase Instance;
    public List<TalentData> allTalents;

    private void Awake() => Instance = this;

    public TalentData GetTalentByID(int id)
    {
        return allTalents.Find(t => t.talentID == id);
    }

    public List<TalentData> GetAvailableTalents()
    {
        var result = new List<TalentData>();

        foreach (var talent in allTalents)
        {
            if (!TalentManager.Instance.ownedTalentIDs.Contains(talent.talentID)
                && TalentManager.Instance.CanSelect(talent))
            {
                result.Add(talent);
            }
        }

        return result;
    }
}
