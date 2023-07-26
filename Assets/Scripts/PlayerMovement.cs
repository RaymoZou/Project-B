using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

  Rigidbody2D rb;
  Animator myAnimator;
  SpriteRenderer spriteRenderer;

  [SerializeField] GameObject groundCheck;

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

  [Header("Dash")]
  [SerializeField] float dashForce = 5.0f;
  [SerializeField] bool isDashing = false;
  [SerializeField] float dashTime = 0.1f;

  private float xInput;
  private bool isJumping = false;
  private float currJumpTime;
  private float currDashTime;

  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    myAnimator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update() {
    xInput = Input.GetAxisRaw("Horizontal");
    if (Input.GetButtonDown("Jump") && isGrounded() && !isJumping) {
      rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
      isJumping = true;
      currJumpTime = maxJumpTime;
    }

    if (Input.GetButtonDown("Left Shift")) {
      isDashing = true;
      rb.gravityScale = 0;
      currDashTime = dashTime;
    }

    if (Input.GetButtonUp("Jump") && currJumpTime > 0.01f) {
      Vector2 tempDownward = Vector2.down * downwardForce * (currJumpTime / maxJumpTime);
      rb.AddForce(tempDownward, ForceMode2D.Impulse);
    }

    #region Animation / Visuals
    if (myAnimator == null) return;
    myAnimator.SetBool("isJump", !isGrounded());
    myAnimator.SetBool("isWalking", xInput != 0 ? true : false);
    myAnimator.SetBool("isDash", isDashing);
    if (xInput == 1) spriteRenderer.flipX = false;
    if (xInput == -1) spriteRenderer.flipX = true;
    #endregion
  }

  // FixedUpdate is called once every 0.02 seconds or 50 times/second by default
  private void FixedUpdate() {
    #region Horizontal Movement

    if (isDashing) {
      float orientation = spriteRenderer.flipX ? -1 : 1;
      rb.AddForce(orientation * dashForce * Vector2.right, ForceMode2D.Impulse);
      currDashTime -= Time.deltaTime;
      if (currDashTime < 0) {
        rb.gravityScale = gravityScale;
        isDashing = false;
        currDashTime = 0;
      }
    } else {
      float targetVelocity = xInput * topSpeed;
      float speedDiff = targetVelocity - rb.velocity.x;
      float accelRate = (Mathf.Abs(targetVelocity) > 0.01f) ? accelerationRate : deaccelerationRate;
      float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);
      rb.AddForce(movement * Vector2.right);
    }
    #endregion

    if (Input.GetButton("Jump") && currJumpTime > 0f && isJumping) {
      rb.AddForce(Vector2.up * jumpForce);
      isJumping = true;
      currJumpTime -= Time.deltaTime;
    } else {
      isJumping = false;
    }

    rb.gravityScale = rb.velocity.y < 0 ? gravityScale * gravityMultiplier : gravityScale;
    if (rb.velocity.y < -maxFallSpeed) rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
  }

  private bool isGrounded() {
    return groundCheck.GetComponent<GroundCheck>().isGrounded;
  }
}
