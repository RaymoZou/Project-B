using System;
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
  Animator myAnimator;
  SpriteRenderer spriteRenderer;

  [Header("Ground check")]
  private bool isGrounded = false;
  const float GROUND_CHECK_OFFSET = -0.50f; // relative to player position
  const float MIN_GROUND_DISTANCE = 0.1f;

  [Header("Movement Settings")]
  const float TOP_SPEED = 4f;
  const float ACCEL_RATE = 7.5f;
  const float DEACCEL_RATE = 5f;
  const float JUMP_FORCE = 7f;
  const float MAX_JUMP_TIME = 0.35f;
  const float GRAVITY_MULTIPLIER = 1.25f;
  const float GRAVITY_SCALE = 1f;
  const float VEL_POWER = 1.25f;
  const float MAX_FALL_SPEED = 10.0f;
  const float WALL_JUMP_CD = 0.2f;
  public float WALL_JUMP_X_FORCE = 16.0f; // TODO: change this const
  public float WALL_JUMP_Y_FORCE = 8.0f;
  private bool isJumping = false;
  public bool isTouchingWall = false;
  private float xInput;
  private float currJumpTime;

  [Header("Dash")]
  const float DASH_FORCE = 5.0f;
  const float DASH_DURATION = 0.1f;
  const float DASH_COOLDOWN = 2f;
  private bool isDashing = false;
  private float currDashCooldown;
  public static event Action<float, int> OnDashChange;
  private float currDashDuration;

  [Header("Player Input")]
  private PlayerInput playerInput;
  private bool isJumpInput;
  private bool isDashingInput;
  private Vector2 currDirection;
  const float WALL_DETECTOR_LENGTH = 0.27f; // wall detection in front of the player
  public float lastWallJump = 0f;

  private Interactable currInteractable;

  private void OnJump(InputAction.CallbackContext context) { isJumpInput = true; }
  private void OnJumpRelease(InputAction.CallbackContext context) { isJumpInput = false; }
  private void OnDash(InputAction.CallbackContext context) { isDashingInput = true; }
  private void OnDashRelease(InputAction.CallbackContext context) { isDashingInput = false; }

  private void SubscribeEvents() {
    playerInput.actions["Jump"].started += OnJump;
    playerInput.actions["Jump"].canceled += OnJumpRelease;
    playerInput.actions["Dash"].started += OnDash;
    playerInput.actions["Dash"].canceled += OnDashRelease;
  }

  private void Awake() {
    playerInput = GetComponent<PlayerInput>();
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    myAnimator = GetComponent<Animator>();
    SubscribeEvents();
  }

  // private void OnCollisionStay2D(Collision2D collider) {
  //   if (!(collider.gameObject.layer == LayerMask.NameToLayer("Wall"))) return;
  //   isTouchingWall = true;
  // }

  // private void OnCollisionExit2D(Collision2D collider) {
  //   isTouchingWall = false;
  // }

  // Update is called once per frame
  void Update() {

    currDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

    lastWallJump += Time.deltaTime; // increment the wall jump timer
    if (lastWallJump < WALL_JUMP_CD) {
      xInput = 0;
    } else {
      xInput = playerInput.actions["Move"].ReadValue<Vector2>().x;
    }

    if (Input.GetButtonDown("Interact")) {
      if (currInteractable) currInteractable.Interact();
    }

    #region Dash
    if (isDashingInput && currDashCooldown < 0) {
      OnDashChange?.Invoke(DASH_COOLDOWN, gameObject.layer);
      currDashCooldown = DASH_COOLDOWN;
      isDashing = true;
      rb.gravityScale = 0;
      currDashDuration = DASH_DURATION;
    }
    currDashCooldown -= Time.deltaTime;
    #endregion

    #region Jump / Ground Check
    // initial jump force
    if (isJumpInput && isGrounded && !isJumping) {
      rb.AddForce(Vector2.up * JUMP_FORCE, ForceMode2D.Impulse);
      isJumping = true;
      currJumpTime = MAX_JUMP_TIME;
    }

    isGrounded = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + GROUND_CHECK_OFFSET), Vector2.down, MIN_GROUND_DISTANCE, LayerMask.GetMask("Ground", "Wall"));
    // Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + GROUND_CHECK_OFFSET), Vector2.down * MIN_GROUND_DISTANCE, Color.green);
    #endregion

    #region Animation / Visuals
    if (myAnimator == null) return;
    myAnimator.SetBool("isJump", !isGrounded);
    myAnimator.SetBool("isWalking", xInput != 0);
    myAnimator.SetBool("isDash", isDashing);
    if (xInput > 0) spriteRenderer.flipX = false;
    if (xInput < 0) spriteRenderer.flipX = true;
    #endregion
  }

  // FixedUpdate is called once every 0.02 seconds or 50 times/second by default
  private void FixedUpdate() {
    #region Wall Jump
    RaycastHit2D wall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), currDirection, WALL_DETECTOR_LENGTH, LayerMask.GetMask("Wall"));
    isTouchingWall = wall;
    // Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), currDirection * WALL_DETECTOR_LENGTH, Color.red);
    // wall jump
    if (isTouchingWall && isJumpInput) {
      rb.velocity = new(wall.normal.x * WALL_JUMP_X_FORCE, WALL_JUMP_Y_FORCE);
      lastWallJump = 0; /// reset jump timer
      spriteRenderer.flipX = !spriteRenderer.flipX; // flip the sprite
    }
    #endregion
    #region Horizontal Movement

    if (isDashing) {
      float orientation = spriteRenderer.flipX ? -1 : 1;
      rb.AddForce(orientation * DASH_FORCE * Vector2.right, ForceMode2D.Impulse);
      currDashDuration -= Time.deltaTime;
      if (currDashDuration < 0) {
        rb.gravityScale = GRAVITY_SCALE;
        isDashing = false;
        currDashDuration = 0;
      }
    } else {
      float targetVelocity = xInput * TOP_SPEED;
      float speedDiff = targetVelocity - rb.velocity.x;
      float accelRate = (Mathf.Abs(targetVelocity) > 0.01f) ? ACCEL_RATE : DEACCEL_RATE;
      float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, VEL_POWER) * Mathf.Sign(speedDiff);
      rb.AddForce(movement * Vector2.right);
    }
    #endregion

    // continuously add force each frame while jump key is held down
    if (isJumpInput && currJumpTime > 0f && isJumping) {
      rb.AddForce(Vector2.up * JUMP_FORCE);
      currJumpTime -= Time.deltaTime;
    } else {
      isJumping = false;
    }

    rb.gravityScale = rb.velocity.y < 0 ? GRAVITY_SCALE * GRAVITY_MULTIPLIER : GRAVITY_SCALE;
    if (rb.velocity.y < -MAX_FALL_SPEED) rb.velocity = new Vector2(rb.velocity.x, -MAX_FALL_SPEED);
  }

  public void SetInteractable(Interactable interactable) {
    currInteractable = interactable;
  }
}
