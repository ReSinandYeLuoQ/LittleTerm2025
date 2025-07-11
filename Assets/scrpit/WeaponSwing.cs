using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector3 dirToMouse = (mouseWorld - player.position).normalized;

        // 玩家只左右面向（假设你自己控制Player的flipX）
        if (dirToMouse.x < 0)
        {
            player.localScale = new Vector3(-1, 1, 1); // 朝左
        }
        else
        {
            player.localScale = new Vector3(1, 1, 1);  // 朝右
        }

        switch (state)
        {
            case State.Idle:
                timer += Time.deltaTime;

                // 停留状态，武器位置贴玩家侧边一点，方向朝鼠标自由角度
                attackDirection = dirToMouse;
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

        // 武器旋转自由朝向鼠标（无死角）
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}
