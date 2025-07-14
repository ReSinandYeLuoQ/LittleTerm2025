using UnityEngine;

public class playerface : MonoBehaviour
{
    public SpriteRenderer characterRenderer;
    public Animator characterAnimator;

    private bool facingRight = true;

    void Update()
    {
        HandleFlipByWASD();
        UpdateAnimationState();
    }

    private void HandleFlipByWASD()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal > 0 && !facingRight)
        {
            Flip(true); // ≥Ø”“
        }
        else if (horizontal < 0 && facingRight)
        {
            Flip(false); // ≥Ø◊Û
        }
    }

    private void Flip(bool faceRight)
    {
        facingRight = faceRight;
        characterRenderer.flipX = !faceRight;
    }

    private void UpdateAnimationState()
    {
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        characterAnimator.SetBool("IsMoving", isMoving);
    }
}
