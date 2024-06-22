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
  private float minDistanceOffGround = 0.1f;

  [Header("Movement Settings")]
  [SerializeField] float topSpeed = 4f;
  [SerializeField] float accelerationRate = 7.5f;
  [SerializeField] float deaccelerationRate = 5f;
  [SerializeField] float jumpForce = 7f;
  [SerializeField] float maxJumpTime = 0.35f;
  [SerializeField] float gravityMultiplier = 1.25f;
  [SerializeField] float gravityScale = 1f;
  [SerializeField] float downwardForce = 2f;
  [SerializeField] float velPower = 1.25f;
  [SerializeField] float maxFallSpeed = 10.0f;
  private bool isJumping = false;
  private float xInput;
  private float currJumpTime;

  [Header("Dash")]
  [SerializeField] float dashForce = 5.0f;
  [SerializeField] bool isDashing = false;
  [SerializeField] float dashDuration = 0.1f;
  [SerializeField] float dashCooldown = 2f;
  [SerializeField] float currDashCooldown;
  public static event Action<float> OnDashChange;
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
      OnDashChange?.Invoke(dashCooldown);
      currDashCooldown = dashCooldown;
      isDashing = true;
      rb.gravityScale = 0;
      currDashDuration = dashDuration;
    }
    currDashCooldown -= Time.deltaTime;
    #endregion

    #region Jump / Ground Check
    // Refactor into the FixedUpdate - DO NOT ADDFORCE IN UPDATE METHOD
    if (isJumpInput && isGrounded && !isJumping)
    {
      rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
      isJumping = true;
      currJumpTime = maxJumpTime;
    }

    isGrounded = Physics2D.Raycast(groundCheck.transform.position, Vector2.down, minDistanceOffGround, LayerMask.GetMask("Ground"));
    Debug.DrawRay(groundCheck.transform.position, Vector2.down * minDistanceOffGround, Color.green);
    #endregion

    #region Animation / Visuals
    if (myAnimator == null) return;
    myAnimator.SetBool("isJump", !isGrounded);
    myAnimator.SetBool("isWalking", xInput != 0 ? true : false);
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
      rb.AddForce(orientation * dashForce * Vector2.right, ForceMode2D.Impulse);
      currDashDuration -= Time.deltaTime;
      if (currDashDuration < 0)
      {
        rb.gravityScale = gravityScale;
        isDashing = false;
        currDashDuration = 0;
      }
    } else
    {
      float targetVelocity = xInput * topSpeed;
      float speedDiff = targetVelocity - rb.velocity.x;
      float accelRate = (Mathf.Abs(targetVelocity) > 0.01f) ? accelerationRate : deaccelerationRate;
      float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);
      rb.AddForce(movement * Vector2.right);
    }
    #endregion

    if (isJumpInput && currJumpTime > 0f && isJumping)
    {
      rb.AddForce(Vector2.up * jumpForce);
      isJumping = true;
      currJumpTime -= Time.deltaTime;
    } else
    {
      isJumping = false;
    }

    rb.gravityScale = rb.velocity.y < 0 ? gravityScale * gravityMultiplier : gravityScale;
    if (rb.velocity.y < -maxFallSpeed) rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
  }

  public void SetInteractable(Interactable interactable)
  {
    currInteractable = interactable;
  }
}