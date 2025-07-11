using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentOptionUI : MonoBehaviour
{
    public Image icon;
    public Text nameText;
    public Text descText;

    private TalentData data;

    public void Setup(TalentData talent)
    {
        data = talent;
        icon.sprite = data.icon;
        nameText.text = data.talentName;
        descText.text = data.description;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            FindObjectOfType<LevelUpUI>().SelectTalent(data);
        });
    }
}
