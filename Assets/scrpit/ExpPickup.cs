using UnityEngine;

public class ExpPickup : MonoBehaviour
{
    public int expValue = 1; // 每个经验值

    private Transform player;
    public float moveSpeed = 5f;
    public float attractDistance = 2.5f; // 吸引的距离

    // 添加 AudioSource 引用
    public AudioSource pickupSound;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // 如果没有手动赋值，尝试获取组件
        if (pickupSound == null)
        {
            pickupSound = GetComponent<AudioSource>();
        }
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
                // 播放拾取音效
                if (pickupSound != null)
                {
                    pickupSound.Play();
                }
            }

            Destroy(gameObject);
        }
    }
}
