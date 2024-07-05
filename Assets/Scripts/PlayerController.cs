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
  Animator animator;
  SpriteRenderer spriteRenderer;

  const float GROUND_CHECK_OFFSET = -0.50f; // relative to player position
  const float MIN_GROUND_DISTANCE = 0.1f;

  const float TOP_SPEED = 4f;
  const float ACCEL_RATE = 7.5f;
  const float DEACCEL_RATE = 5f;
  const float JUMP_FORCE = 7f;
  const float MAX_JUMP_TIME = 0.35f;
  const float VEL_POWER = 1.25f;
  const float MAX_FALL_SPEED = 10.0f;
  const float WALL_JUMP_CD = 0.2f;
  const float WALL_JUMP_X_FORCE = 16.0f;
  const float WALL_JUMP_Y_FORCE = 8.0f;
  const float DASH_FORCE = 5.0f;
  const float DASH_DURATION = 0.1f;
  const float DASH_CD = 2f;
  const float WALL_DETECTOR_LENGTH = 0.27f; // wall detection in front of the player

  // player state
  private bool isJumping = false;
  // public bool isTouchingWall = false;
  private bool isGrounded = false;
  private bool isDashing = false;
  private float xInput;
  private float currJumpTime;
  private float currDashCD;
  public float lastWallJump = 0f;
  private float currDashDuration;
  private Vector2 currDirection;
  public static event Action<float, int> OnDashChange;

  private PlayerInput playerInput;
  private bool jumpInput;
  private bool dashInput;
  private Interactable currInteractable;

  private void Awake() {
    playerInput = GetComponent<PlayerInput>();
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    animator = GetComponent<Animator>();
  }


  private void PollInput() {
    jumpInput = playerInput.actions["Jump"].inProgress; // jump input
    dashInput = playerInput.actions["Dash"].inProgress; // jump input
    xInput = playerInput.actions["Move"].ReadValue<Vector2>().x; // x input
    if (lastWallJump < WALL_JUMP_CD) xInput = 0; // (cancel x input)

    if (Input.GetButtonDown("Interact")) {
      if (currInteractable) currInteractable.Interact();
    }

  }
  void Update() {
    PollInput();
    // decrement timers
    currDashCD -= Time.deltaTime;
    lastWallJump += Time.deltaTime; // increment the wall jump timer
    currDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;


    #region Dash
    if (dashInput && currDashCD < 0) {
      OnDashChange?.Invoke(DASH_CD, gameObject.layer);
      currDashCD = DASH_CD;
      isDashing = true;
      currDashDuration = DASH_DURATION;
    }
    #endregion

    #region Jump / Ground Check
    // initial jump force
    if (jumpInput && isGrounded && !isJumping) {
      rb.AddForce(Vector2.up * JUMP_FORCE, ForceMode2D.Impulse);
      isJumping = true;
      currJumpTime = 0;
    }

    isGrounded = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + GROUND_CHECK_OFFSET), Vector2.down, MIN_GROUND_DISTANCE, LayerMask.GetMask("Ground", "Wall"));
    // Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + GROUND_CHECK_OFFSET), Vector2.down * MIN_GROUND_DISTANCE, Color.green);
    #endregion

    #region Wall Jump
    RaycastHit2D wall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), currDirection, WALL_DETECTOR_LENGTH, LayerMask.GetMask("Wall"));
    // wall jump
    if (wall && jumpInput) {
      rb.velocity = new(wall.normal.x * WALL_JUMP_X_FORCE, WALL_JUMP_Y_FORCE);
      lastWallJump = 0; /// reset jump timer
      spriteRenderer.flipX = !spriteRenderer.flipX; // flip the sprite
    }

    #endregion
    #region Animation / Visuals
    animator.SetBool("isJump", !isGrounded);
    animator.SetBool("isWalking", xInput != 0);
    animator.SetBool("isDash", isDashing);
    if (xInput > 0) spriteRenderer.flipX = false;
    if (xInput < 0) spriteRenderer.flipX = true;
    #endregion
  }

  // FixedUpdate is called once every 0.02 seconds or 50 times/second by default
  private void FixedUpdate() {
    #region Horizontal Movement

    // dash impulse force should be applied in one frame
    if (isDashing) {
      float orientation = spriteRenderer.flipX ? -1 : 1;
      rb.AddForce(orientation * DASH_FORCE * Vector2.right, ForceMode2D.Impulse);
      currDashDuration -= Time.deltaTime;
      if (currDashDuration < 0) {
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
    if (jumpInput && currJumpTime < MAX_JUMP_TIME && isJumping) {
      rb.AddForce(Vector2.up * JUMP_FORCE);
      currJumpTime += Time.deltaTime;
    } else {
      isJumping = false;


      // clamp fall speed
      if (rb.velocity.y < -MAX_FALL_SPEED) rb.velocity = new Vector2(rb.velocity.x, -MAX_FALL_SPEED);
    }
  }

  public void SetInteractable(Interactable interactable) {
    currInteractable = interactable;
  }
}
