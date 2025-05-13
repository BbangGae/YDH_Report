using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerBaseController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool canMove = true;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector2 moveInput;
    protected bool facingRight = true;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (!canMove)
        {
            moveInput = Vector2.zero;
            UpdateAnimation(false);
            return;
        }

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        bool isMoving = moveInput.magnitude > 0.01f;
        UpdateAnimation(isMoving);

        if (moveInput.x > 0.01f && !facingRight)
            Flip(true);
        else if (moveInput.x < -0.01f && facingRight)
            Flip(false);
    }

    protected virtual void FixedUpdate()
    {
        if (canMove)
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    protected void Flip(bool faceRight)
    {
        facingRight = faceRight;
        Vector3 scale = transform.localScale;
        scale.x = faceRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    protected void UpdateAnimation(bool isMoving)
    {
        // ✅ 애니메이터와 컨트롤러 모두 존재할 때만 애니메이션 변경
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetBool("isMoving", isMoving);
        }
    }
}
