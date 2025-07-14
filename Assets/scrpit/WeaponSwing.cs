using UnityEngine;

public class WeaponSwing : MonoBehaviour
{
    public Transform player;

    public float attackDuration = 1f;
    public float returnDuration = 1f;
    public float restDuration = 1f;
    public float swingDistance = 3f;
    public AnimationCurve swingCurve;

    private float timer = 0f;
    private enum State { Idle, Attacking, Returning }
    private State state = State.Idle;

    private Vector3 attackDirection;

    void Start()
    {
        if (swingCurve == null || swingCurve.length == 0)
        {
            swingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }

        transform.position = player.position;
    }

    void Update()
    {
        // 鼠标方向（始终指向）
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

        // 设置旋转：武器底部朝向玩家，顶部朝向鼠标方向
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // -90 是为了让模型“底部”朝向玩家
    }
}
