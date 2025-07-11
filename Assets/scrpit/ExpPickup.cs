using UnityEngine;

public class ExpPickup : MonoBehaviour
{
    public int expValue = 1; // 每个经验点数

    private Transform player;
    public float moveSpeed = 5f;
    public float attractDistance = 2.5f; // 被吸附的距离

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < attractDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerexp playerExp = other.GetComponent<playerexp>();
            if (playerExp != null)
            {
                playerExp.AddExp(expValue);
            }

            Destroy(gameObject);
        }
    }
}
