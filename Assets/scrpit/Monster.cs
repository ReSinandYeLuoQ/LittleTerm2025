using UnityEngine;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

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

    public AudioClip deathSound; // 拖音效资源进来
    private AudioSource audioSource;




    void Start()
    {
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }



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

    public void TakeDamage(float dmg)
    {
        if (isInvincible) return;

        currentHP -= (int)dmg;

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
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

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
        PlayerStats stats = player.GetComponent<PlayerStats>();
        if (other.CompareTag("Weapon"))
        {
            TakeDamage(13f* (stats.attackPowerMultiplier));
        }
    }
}
