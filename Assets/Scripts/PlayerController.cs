using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {

  Rigidbody2D rb;
  Animator animator;
  SpriteRenderer spriteRenderer;

  // const float GROUND_CHECK_OFFSET = -0.50f; // relative to player position
  // const float MIN_GROUND_DISTANCE = 0.1f;

  const float TOP_SPEED = 4f;
  const float ACCEL_RATE = 7.5f;
  const float DEACCEL_RATE = 5f;
  const float VEL_POWER = 1.25f;
  const float MAX_FALL_SPEED = 10.0f;
  const float WALL_JUMP_CD = 0.2f;
  const float WALL_JUMP_X_FORCE = 16.0f;
  const float WALL_JUMP_Y_FORCE = 8.0f;
  const float DASH_FORCE = 6.0f;
  const float DASH_DURATION = 0.1f;
  const float DASH_CD = 2f;
  // const float WALL_DETECTOR_LENGTH = 0.27f; // wall detection in front of the player

  // player state
  public bool isGrounded = false;
  public bool isWall;
  public bool isDashing = false;
  private float xInput;
  public float lastWallJump = 0f;
  private Vector2 currDirection;
  public static event Action<float, int> OnDashChange;
  private float timeSinceGrounded;

  private PlayerInput playerInput;
  public bool jumpInput;
  private bool dashInput;
  public int jumpCount;
  public bool canDash = true;
  private Interactable currInteractable;
  public float MAX_JUMP_TIME = 0.35f;
  public float JUMP_FORCE = 8f;
  public bool isJumping = false;
  public float jumpTime;
  [SerializeField] BoxCollider2D wallCheck;
  [SerializeField] BoxCollider2D groundCheck;
  private void Awake() {
    playerInput = GetComponent<PlayerInput>();
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    animator = GetComponent<Animator>();
  }


  private void PollInput() {
    jumpInput = playerInput.actions["Jump"].triggered; // jump input
    dashInput = playerInput.actions["Dash"].triggered; // jump input
    xInput = playerInput.actions["Move"].ReadValue<Vector2>().x; // x input
    if (lastWallJump < WALL_JUMP_CD) xInput = 0; // (cancel x input)

    if (Input.GetButtonDown("Interact")) {
      if (currInteractable) currInteractable.Interact();
    }
  }

  IEnumerator DashCooldown(float duration) {
    canDash = false;
    yield return new WaitForSeconds(duration);
    canDash = true;
  }

  IEnumerator Dash(float duration) {
    isDashing = true;
    yield return new WaitForSeconds(duration);
    isDashing = false;
  }

  // check if player is grounded
  void CheckGrounded() {
    isGrounded = groundCheck.IsTouchingLayers(LayerMask.GetMask("Ground", "Wall"));
    if (isGrounded) {
      isJumping = false;
      jumpCount = 0; // reset jump count
      timeSinceGrounded = 0;
    } else {
      timeSinceGrounded += Time.deltaTime;
    }
    // Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + GROUND_CHECK_OFFSET), Vector2.down * MIN_GROUND_DISTANCE, Color.green);
  }

  void CheckWall() {
    isWall = wallCheck.IsTouchingLayers(LayerMask.GetMask("Ground", "Wall"));
  }

  void WallJump() {
    if (isWall && jumpInput) {
      rb.velocity = new(-transform.localScale.x * WALL_JUMP_X_FORCE, WALL_JUMP_Y_FORCE);
      lastWallJump = 0; /// reset jump timer
      transform.localScale = new(-transform.localScale.x, transform.localScale.y);
    } else {
      lastWallJump += Time.deltaTime;
    }
  }

  void Jump() {
    if (jumpInput && timeSinceGrounded < 0.1f) {
      rb.AddForce(Vector2.up * JUMP_FORCE, ForceMode2D.Impulse);
    }
  }

  void UpdateVisuals() {
    animator.SetBool("isJump", !isGrounded);
    animator.SetBool("isWalking", xInput != 0);
    animator.SetBool("isDash", isDashing);
    animator.SetBool("isSlide", isWall);
    if (xInput < 0) transform.localScale = new Vector2(-1, transform.localScale.y);
    if (xInput > 0) transform.localScale = new Vector2(1, transform.localScale.y);
    // if (xInput > 0) spriteRenderer.flipX = false;
    // if (xInput < 0) spriteRenderer.flipX = true;
  }

  void Dash() {
    if (dashInput && canDash) {
      StartCoroutine(DashCooldown(DASH_CD));
      StartCoroutine(Dash(DASH_DURATION));
      OnDashChange?.Invoke(DASH_CD, gameObject.layer);
    }
  }

  void Update() {
    currDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left; // get direction
    PollInput();
    CheckGrounded();
    Dash();
    Jump();
    CheckWall();
    WallJump();
    UpdateVisuals();
  }

  // FixedUpdate is called once every 0.02 seconds or 50 times/second by default
  private void FixedUpdate() {
    // if player is dashing, then surrender movement for duration of dash
    if (isDashing) {
      rb.velocity = new(rb.velocity.x, 0); // freeze the y velocity of player during dash
      float orientation = transform.localScale.x < 0 ? -1 : 1;
      rb.AddForce(orientation * DASH_FORCE * Vector2.right, ForceMode2D.Impulse);
    }

    // regular movement
    if (!isDashing) {
      float targetVelocity = xInput * TOP_SPEED;
      float speedDiff = targetVelocity - rb.velocity.x;
      float accelRate = (Mathf.Abs(targetVelocity) > 0.01f) ? ACCEL_RATE : DEACCEL_RATE;
      float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, VEL_POWER) * Mathf.Sign(speedDiff);
      rb.AddForce(movement * Vector2.right);
    }

    if (rb.velocity.y < -MAX_FALL_SPEED) rb.velocity = new Vector2(rb.velocity.x, -MAX_FALL_SPEED);
  }

  public void SetInteractable(Interactable interactable) {
    currInteractable = interactable;
  }
}
