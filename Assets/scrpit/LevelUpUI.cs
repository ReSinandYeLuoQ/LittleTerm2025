using System.Collections.Generic;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    public Transform optionContainer; // 放置天赋选项的容器
    public GameObject talentOptionPrefab;
    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);

        var candidates = TalentDatabase.Instance.GetAvailableTalents();
        var options = new List<TalentData>();

        // 随机选出最多3个合法天赋
        while (options.Count < 3 && candidates.Count > 0)
        {
            int index = Random.Range(0, candidates.Count);
            options.Add(candidates[index]);
            candidates.RemoveAt(index);
        }

        foreach (Transform child in optionContainer)
            Destroy(child.gameObject);

        foreach (var talent in options)
        {
            var go = Instantiate(talentOptionPrefab, optionContainer);
            go.GetComponent<TalentOptionUI>().Setup(talent);
        }
    }

    public void SelectTalent(TalentData talent)
    {
        TalentManager.Instance.LearnTalent(talent);
        Close();
    }

    public void Close()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
