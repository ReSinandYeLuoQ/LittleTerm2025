using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    // 修正类型为 Animator
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

    void Start()
    {
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // 在 Start 方法中初始化 anim
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        // 根据玩家的位置判断是否需要翻转怪物
        if (dir.x < 0) // 玩家在怪物右侧
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (dir.x > 0) // 玩家在怪物左侧
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // 移除错误的初始化代码
        // anmi = GetComponent<Animater>();
    }

    public void TakeDamage(int dmg)
    {
        if (isInvincible) return;

        currentHP -= dmg;

        if (currentHP <= 0)
        {
            // 设置动画状态
            if (anim != null)
            {
                anim.SetBool("isdie", true);
            }
            // 启动协程等待 1 秒后调用 Die 方法
            StartCoroutine(WaitAndDie(1f));
            // 移除原有的立即调用 Die 方法
            // Die();
        }
        else
        {
            StartCoroutine(Invincibility());
        }
    }

    // 新增协程方法，等待指定时间后调用 Die 方法
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

        if (Random.value < 0.2f) // 20% ����
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
