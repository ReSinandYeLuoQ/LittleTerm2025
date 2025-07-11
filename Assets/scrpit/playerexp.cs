using UnityEngine;

public class playerexp : MonoBehaviour
{
    public int currentExp = 0;
    public int level = 1;
    public int expToNextLevel = 1;


    public LevelUpUI levelUpUI; // 关联升级界面脚本

    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log($"获得经验：{amount}，当前总经验：{currentExp}");

        while (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentExp -= expToNextLevel;
        level++;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.5f);
        Debug.Log($"升级！当前等级：{level}，下一级需要经验：{expToNextLevel}");
        levelUpUI.Show();
        // 弹出升级界面
            
    }
}
