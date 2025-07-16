using UnityEngine;

public class WeaponSwing : MonoBehaviour
{
    public Transform player;
    public Animator animator; // 👈 新增：动画控制器

    public float baseAttackDuration = 1f;
    public float baseReturnDuration = 1f;
    public float baseRestDuration = 1f;
    public float swingDistance = 3f;
    public AnimationCurve swingCurve;

    private float attackDuration;
    private float returnDuration;
    private float restDuration;

    private float timer = 0f;
    private enum State { Idle, Attacking, Returning }
    private State state = State.Idle;

    private Vector3 attackDirection;
    private PlayerStats stats; // 👈 新增

    void Start()
    {
        if (swingCurve == null || swingCurve.length == 0)
        {
            swingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }

        if (player != null)
        {
            transform.position = player.position;
            stats = player.GetComponent<PlayerStats>();
        }

        UpdateDurations();
    }

    void Update()
    {
        if (stats != null)
        {
            UpdateDurations(); // 实时调整持续时间
        }

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        attackDirection = (mouseWorld - player.position).normalized;

        switch (state)
        {
            case State.Idle:
                timer += Time.deltaTime;
                transform.position = player.position + attackDirection * 0.5f;

                if (timer >= restDuration)
                {
                    timer = 0f;
                    state = State.Attacking;

                    if (animator != null)
                        animator.SetTrigger("Swing"); // 👈 播放动画（只在攻击阶段）
                }
                break;

            case State.Attacking:
                timer += Time.deltaTime;
                float t1 = timer / attackDuration;
                if (t1 >= 1f)
                {
                    timer = 0f;
                    state = State.Returning;
                }
                else
                {
                    float eval = swingCurve.Evaluate(t1);
                    transform.position = player.position + attackDirection * swingDistance * eval;
                }
                break;

            case State.Returning:
                timer += Time.deltaTime;
                float t2 = timer / returnDuration;
                if (t2 >= 1f)
                {
                    timer = 0f;
                    state = State.Idle;
                }
                else
                {
                    float eval = swingCurve.Evaluate(1f - t2);
                    transform.position = player.position + attackDirection * swingDistance * eval;
                }
                break;
        }

        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    void UpdateDurations()
    {
        float multiplier = stats != null ? stats.attackSpeedMultiplier : 1f;
        multiplier = Mathf.Max(0.1f, multiplier); // 防止除0

        attackDuration = baseAttackDuration / multiplier;
        returnDuration = baseReturnDuration / multiplier;
        restDuration = baseRestDuration / multiplier;
    }
}
