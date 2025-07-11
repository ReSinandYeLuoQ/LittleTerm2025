using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
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

    void Start()
    {
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    public void TakeDamage(int dmg)
    {
        if (isInvincible) return;

        currentHP -= dmg;

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Invincibility());
        }
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    void Die()
    {
        //animator.SetTrigger("Die");
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

        if (Random.value < 0.2f) // 20% µôÂä
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
