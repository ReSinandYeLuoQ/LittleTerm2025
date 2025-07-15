using UnityEngine;

public class enemydamage : MonoBehaviour
{
    public Color hurtColor = Color.red;
    public float flashDuration = 1f;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // 造成伤害
            PlayerStats stats = collision.collider.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(20);
                // 让玩家变红（前提是玩家脚本里也加了变红功能）
                stats.FlashRed();
            }

            // ❌ 去掉怪物被击退的逻辑（以下为原代码，已删除）
            // Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
            // Rigidbody2D rb = GetComponent<Rigidbody2D>();
            // if (rb != null)
            // {
            //     rb.velocity = knockbackDir * knockbackForce;
            // }

            // 自己变红
            if (spriteRenderer != null)
            {
                StopAllCoroutines(); // 防止叠加
                StartCoroutine(FlashRedSelf());
            }
        }
    }

    private System.Collections.IEnumerator FlashRedSelf()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = hurtColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }
}
