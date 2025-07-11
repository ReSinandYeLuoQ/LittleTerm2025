using UnityEngine;

public class PlayerBar : MonoBehaviour
{
    public Transform healthFill;
    public Transform expFill;

    [Range(0, 1)] public float healthPercent = 1f;
    [Range(0, 1)] public float expPercent = 0f;

    void Update()
    {
        // 实时更新填充条缩放
        if (healthFill != null)
            healthFill.localScale = new Vector3(healthPercent, 1f, 1f);

        if (expFill != null)
            expFill.localScale = new Vector3(expPercent, 1f, 1f);
    }

    public void SetHealth(float percent)
    {
        healthPercent = Mathf.Clamp01(percent);
    }

    public void SetExp(float percent)
    {
        expPercent = Mathf.Clamp01(percent);
    }
}
