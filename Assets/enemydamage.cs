using UnityEngine;

public class enemydamage : MonoBehaviour
{
    public float knockbackForce = 15f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // ����˺�
            PlayerStats stats = collision.collider.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(20);
            }

            // ���㷴������
            Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;

            // ��ӻ���
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = knockbackDir * knockbackForce;
            }
        }
    }
}
