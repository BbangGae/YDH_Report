using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform;

    private CharacterController controller;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isSprinting;

    private Animator animator;

    [Header("🧪 테스트 옵션")]
    [SerializeField] private bool infiniteJumpEnabled = false;

    [Header("점프 사운드")]
    public AudioSource sfxSource;
    public AudioClip jumpSFX;



    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Jump.performed += _ =>
        {
            if (controller.isGrounded)
            {
                OnJump();
                animator.SetTrigger("Jump"); // 🎯 Jump 트리거 발동
            }
        };

        inputActions.Player.Sprint.performed += _ => isSprinting = true;
        inputActions.Player.Sprint.canceled += _ => isSprinting = false;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        GameTimer.Instance?.StartTimer();
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Update() => HandleMovement();

    void HandleMovement()
    {
        // 입력 방향 → 카메라 기준 회전
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        float speedAmount = direction.magnitude;
        animator.SetFloat("Speed", speedAmount); // 🎯 이동 속도 기반 Run/Idle 전환

        if (speedAmount >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            float speed = walkSpeed * (isSprinting ? sprintMultiplier : 1f);
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // 중력 적용
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnJump()
    {
        // 기존에는 controller.isGrounded만 체크
        if (controller.isGrounded || infiniteJumpEnabled)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // 애니메이션 트리거 추가 (있다면)
            if (animator != null)
                animator.SetTrigger("Jump");

            // 점프 사운드 재생
            if (sfxSource != null && jumpSFX != null)
                sfxSource.PlayOneShot(jumpSFX);
        }
    }


    public void ApplyExternalJumpForce(float force)
    {
        velocity.y = Mathf.Sqrt(force * -2f * gravity);

        if (animator != null)
            animator.SetTrigger("Jump");
    }

}
