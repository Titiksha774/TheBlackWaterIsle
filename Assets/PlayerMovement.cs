using UnityEngine;
using UnityEngine.UI; // for the stamina bar

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpHeight = 0f;
    public float gravity = -9.81f;
    public float jumpCooldown = 2f;

    [Header("Camera & Animator")]
    public Transform cameraTransform;
    public Animator animator;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 20f;   // per second while running
    public float staminaRegenRate = 10f;   // per second while not running
    public Slider staminaBar;              // assign this in inspector

    private CharacterController controller;
    private Vector3 velocity;
    private bool isJumping;
    private bool isRunningJump;
    private float lastJumpTime = -999f;
    private float currentStamina;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        currentStamina = maxStamina;

        // initialize slider if assigned
        if (staminaBar != null)
            staminaBar.value = 1f;
    }

    void Update()
    {
        HandleMovement();
        HandleAnimations();
        UpdateStaminaBar();
    }

    void HandleMovement()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = (h != 0 || v != 0);

        // determine if player can actually run
        bool canRun = wantsToRun && currentStamina > 0f;
        float speed = canRun ? runSpeed : walkSpeed;

        // drain or regenerate stamina
        if (canRun && isMoving)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina < 0f) currentStamina = 0f;
        }
        else
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
        }

        Vector3 dir = new Vector3(h, 0f, v).normalized;
        if (isMoving)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg +
                                (cameraTransform ? cameraTransform.eulerAngles.y : 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }

        // jump handling
        if (Time.time - lastJumpTime >= jumpCooldown && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            lastJumpTime = Time.time;
            isRunningJump = canRun && isMoving;
            isJumping = !isRunningJump;
            animator.SetBool("IsJumping", isJumping);
            animator.SetBool("IsRunningJump", isRunningJump);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if ((isJumping || isRunningJump) && Time.time - lastJumpTime >= jumpCooldown)
        {
            isJumping = false;
            isRunningJump = false;
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsRunningJump", false);
        }
    }

    void HandleAnimations()
    {
        if (animator == null) return;

        bool isMoving = Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0f;

        if (isJumping || isRunningJump) return;

        int state = 0;
        if (isMoving)
            state = isRunning ? 2 : 1;

        animator.SetInteger("State", state);
    }

    void UpdateStaminaBar()
    {
        if (staminaBar != null)
            staminaBar.value = currentStamina / maxStamina;
    }
}
