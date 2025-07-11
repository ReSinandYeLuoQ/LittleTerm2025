using UnityEngine;

public class playerexp : MonoBehaviour
{
    public int currentExp = 0;
    public int level = 1;
    public int expToNextLevel = 1;


    public LevelUpUI levelUpUI; // ������������ű�

    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log($"��þ��飺{amount}����ǰ�ܾ��飺{currentExp}");

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
        Debug.Log($"��������ǰ�ȼ���{level}����һ����Ҫ���飺{expToNextLevel}");
        levelUpUI.Show();
        // ������������
            
    }
}
