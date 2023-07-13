using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

  Rigidbody2D rb;
  Animator myAnimator;
  SpriteRenderer spriteRenderer;

  [SerializeField] GameObject groundCheck;

  [Header("Movement Settings")]
  [SerializeField] float baseMoveSpeed = 1;
  [SerializeField] float topSpeed = 5;
  [SerializeField] float accelerationRate = 15;
  [SerializeField] float deaccelerationRate = 4;
  [SerializeField] float jumpForce = 4;
  [SerializeField] float jumpTime = 0.25f;
  [SerializeField] float gravityMultiplier = 1.5f;
  [SerializeField] float gravityScale = 1f;
  [SerializeField] float airBourneAccelerationRate = 5f;

  private float xInput;
  private bool isJumping = false;
  private float currJumpTimer;

  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    myAnimator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update() {
    xInput = Input.GetAxisRaw("Horizontal");
    if (Input.GetButtonDown("Jump") && isGrounded() && !isJumping) {
      Jump();
    }

    #region Animation / Visuals
    myAnimator.SetBool("isJump", !isGrounded());
    myAnimator.SetBool("isWalking", xInput != 0 ? true : false);
    if (xInput == 1) spriteRenderer.flipX = false;
    if (xInput == -1) spriteRenderer.flipX = true;
    #endregion
  }

  private void Jump() {
    isJumping = true;
    currJumpTimer = jumpTime;
    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
  }

  // FixedUpdate is called once every 0.02 seconds or 50 times/second by default
  private void FixedUpdate() {
    float yVelocity = rb.velocity.y;
    float xVelocity = rb.velocity.x;
    float targetVelocity = xInput * topSpeed;
    xVelocity = Mathf.MoveTowards(xVelocity, targetVelocity, accelerationRate * Time.deltaTime);
    if (isJumping) {
      if (Input.GetButton("Jump") && currJumpTimer > 0) {
        rb.AddForce(Vector2.up * jumpForce * (jumpTime - currJumpTimer), ForceMode2D.Impulse);
        currJumpTimer -= Time.deltaTime;
      } else {
        isJumping = false;
      }
    }
    //if (!isGrounded() && xInput == 0) {
    //  xVelocity = rb.velocity.x;
    //} else if (!isGrounded() && xInput != 0) {
    //  Debug.Log("sdklfjdsklf");
    //  xVelocity = Mathf.MoveTowards(xVelocity, targetVelocity, airBourneAccelerationRate * Time.deltaTime);
    //}
    //xVelocity = Mathf.Lerp(rb.velocity.x, targetVelocity, airBourneAccelerationRate * Time.deltaTime);
    if (!isGrounded()) {
      if (xInput == 0) {
        xVelocity = rb.velocity.x;
      } else {
        Debug.Log("wiggle");
        xVelocity = xVelocity = Mathf.Lerp(rb.velocity.x, targetVelocity, airBourneAccelerationRate * Time.deltaTime);
      }
    }
    Vector2 movement = new Vector2(xVelocity, rb.velocity.y);
    rb.velocity = movement;

    rb.gravityScale = rb.velocity.y < 0 ? gravityScale * gravityMultiplier : gravityScale;
  }

  private bool isGrounded() {
    return groundCheck.GetComponent<GroundCheck>().isGrounded;
  }
}
