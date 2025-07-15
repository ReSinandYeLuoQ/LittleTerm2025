using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    public Animator anim;

    public int level = 1;
    public float speed = 1f;
    public int maxHP = 3;
    private int currentHP;

    private bool isInvincible = false;
    public float invincibleTime = 0.5f;

    public Animator animator;
    public GameObject expPrefab;
    public GameObject specialItemPrefab;

    private Transform player;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();

        // 初始化贴图组件
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        // 翻转
        if (dir.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (dir.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isInvincible) return;

        currentHP -= dmg;

        // 触发受伤变色
        if (spriteRenderer != null)
            StartCoroutine(FlashRed());

        if (currentHP <= 0)
        {
            if (anim != null)
            {
                anim.SetBool("isdie", true);
            }

            StartCoroutine(WaitAndDie(1f));
        }
        else
        {
            StartCoroutine(Invincibility());
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(1f);
        spriteRenderer.color = originalColor;
    }

    IEnumerator WaitAndDie(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Die();
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        StopAllCoroutines();
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(0.5f);
        DropLoot();
        Destroy(gameObject);
    }

    void DropLoot()
    {
        Vector2 offset = Random.insideUnitCircle.normalized * 0.2f;
        Instantiate(expPrefab, transform.position + (Vector3)offset, Quaternion.identity);

        if (Random.value < 0.2f)
        {
            offset = Random.insideUnitCircle.normalized * 0.2f;
            Instantiate(specialItemPrefab, transform.position + (Vector3)offset, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            TakeDamage(1);
        }
    }
}
