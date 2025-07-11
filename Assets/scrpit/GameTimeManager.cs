using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public static float GameTime { get; private set; }

    void Update()
    {
        GameTime += Time.deltaTime;
    }

    public static int GetMonsterLevel()
    {
        float t = GameTime;

        if (t < 60f) return 1;

        if (t < 180f)
        {
            float ratio = (t - 60f) / 120f;
            return Random.value < ratio ? 2 : 1;
        }

        if (t < 300f)
        {
            float ratio = (t - 180f) / 120f;
            return Random.value < ratio ? 3 : 2;
        }

        return 3;
    }
}
