using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

  Rigidbody2D rb;
  Animator myAnimator;
  SpriteRenderer spriteRenderer;

  [Header("Ground check")]
  [SerializeField] Transform groundCheck;
  private bool isGrounded = false;
  private float MIN_GROUND_DISTANCE = 0.1f;

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
  private bool isJumping = false;
  private float xInput;
  private float currJumpTime;

  [Header("Dash")]
  const float DASH_FORCE = 5.0f;
  const float DASH_DURATION = 0.1f;
  const float DASH_COOLDOWN = 2f;
  [SerializeField] bool isDashing = false;
  [SerializeField] float currDashCooldown;
  public static event Action<float, int> OnDashChange;
  private float currDashDuration;

  [Header("Player Input")]
  private PlayerInput playerInput;
  public bool isJumpInput;
  public bool isDashingInput;

  private Interactable currInteractable;

  private void OnJump(InputAction.CallbackContext context) { isJumpInput = true; }
  private void OnJumpRelease(InputAction.CallbackContext context) { isJumpInput = false; }
  private void OnDash(InputAction.CallbackContext context) { isDashingInput = true; }
  private void OnDashRelease(InputAction.CallbackContext context) { isDashingInput = false; }

  private void Awake()
  {
    // TODO: refactor subscription into separate setup method
    playerInput = GetComponent<PlayerInput>();
    playerInput.actions["Jump"].started += OnJump;
    playerInput.actions["Jump"].canceled += OnJumpRelease;
    playerInput.actions["Dash"].started += OnDash;
    playerInput.actions["Dash"].canceled += OnDashRelease;
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    myAnimator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
    xInput = playerInput.actions["Move"].ReadValue<Vector2>().x;

    if (Input.GetButtonDown("Interact"))
    {
      if (currInteractable != null) currInteractable.Interact();
    }

    #region Dash
    if (isDashingInput && currDashCooldown < 0)
    {
      OnDashChange?.Invoke(DASH_COOLDOWN, gameObject.layer);
      currDashCooldown = DASH_COOLDOWN;
      isDashing = true;
      rb.gravityScale = 0;
      currDashDuration = DASH_DURATION;
    }
    currDashCooldown -= Time.deltaTime;
    #endregion

    #region Jump / Ground Check
    // Refactor into the FixedUpdate - DO NOT ADDFORCE IN UPDATE METHOD
    if (isJumpInput && isGrounded && !isJumping)
    {
      rb.AddForce(Vector2.up * JUMP_FORCE, ForceMode2D.Impulse);
      isJumping = true;
      currJumpTime = MAX_JUMP_TIME;
    }

    isGrounded = Physics2D.Raycast(groundCheck.transform.position, Vector2.down, MIN_GROUND_DISTANCE, LayerMask.GetMask("Ground"));
    Debug.DrawRay(groundCheck.transform.position, Vector2.down * MIN_GROUND_DISTANCE, Color.green);
    #endregion

    #region Animation / Visuals
    if (myAnimator == null) return;
    myAnimator.SetBool("isJump", !isGrounded);
    myAnimator.SetBool("isWalking", xInput != 0);
    myAnimator.SetBool("isDash", isDashing);
    if (xInput == 1) spriteRenderer.flipX = false;
    if (xInput == -1) spriteRenderer.flipX = true;
    #endregion
  }

  // FixedUpdate is called once every 0.02 seconds or 50 times/second by default
  private void FixedUpdate()
  {
    #region Horizontal Movement

    if (isDashing)
    {
      float orientation = spriteRenderer.flipX ? -1 : 1;
      rb.AddForce(orientation * DASH_FORCE * Vector2.right, ForceMode2D.Impulse);
      currDashDuration -= Time.deltaTime;
      if (currDashDuration < 0)
      {
        rb.gravityScale = GRAVITY_SCALE;
        isDashing = false;
        currDashDuration = 0;
      }
    } else
    {
      float targetVelocity = xInput * TOP_SPEED;
      float speedDiff = targetVelocity - rb.velocity.x;
      float accelRate = (Mathf.Abs(targetVelocity) > 0.01f) ? ACCEL_RATE : DEACCEL_RATE;
      float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, VEL_POWER) * Mathf.Sign(speedDiff);
      rb.AddForce(movement * Vector2.right);
    }
    #endregion

    if (isJumpInput && currJumpTime > 0f && isJumping)
    {
      rb.AddForce(Vector2.up * JUMP_FORCE);
      isJumping = true;
      currJumpTime -= Time.deltaTime;
    } else
    {
      isJumping = false;
    }

    rb.gravityScale = rb.velocity.y < 0 ? GRAVITY_SCALE * GRAVITY_MULTIPLIER : GRAVITY_SCALE;
    if (rb.velocity.y < -MAX_FALL_SPEED) rb.velocity = new Vector2(rb.velocity.x, -MAX_FALL_SPEED);
  }

  public void SetInteractable(Interactable interactable)
  {
    currInteractable = interactable;
  }
}