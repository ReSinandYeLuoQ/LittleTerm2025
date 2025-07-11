using UnityEngine;

public class enemydamage : MonoBehaviour
{
    public float knockbackForce = 15f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // 造成伤害
            PlayerStats stats = collision.collider.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(20);
            }

            // 计算反弹方向
            Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;

            // 添加击退
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = knockbackDir * knockbackForce;
            }
        }
    }
}
