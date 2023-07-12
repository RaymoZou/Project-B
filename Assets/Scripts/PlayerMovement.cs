using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

  Rigidbody2D rb;
  Animator myAnimator;
  SpriteRenderer spriteRenderer;

  [SerializeField] GameObject groundCheck;

  [Header("Movement Settings")]
  [SerializeField] float moveSpeed = 3;
  [SerializeField] float jumpForce = 4;
  [SerializeField] float jumpTime = 0.25f;

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
      isJumping = true;
      currJumpTimer = jumpTime;
    }

    #region Animation / Visuals
    myAnimator.SetBool("isJump", !isGrounded());
    myAnimator.SetBool("isWalking", xInput != 0 ? true : false);
    if (xInput == 1) spriteRenderer.flipX = false;
    if (xInput == -1) spriteRenderer.flipX = true;
    #endregion
  }

  // FixedUpdate is called once every 0.02 seconds or 50 times/second by default
  private void FixedUpdate() {
    float yVelocity = rb.velocity.y;
    float xVelocity = xInput * moveSpeed;
    if (isJumping) {
      if (Input.GetButton("Jump") && currJumpTimer > 0) {
        yVelocity = jumpForce;
        currJumpTimer -= Time.deltaTime;
      } else {
        isJumping = false;
      }
    }
    Vector2 movement = new Vector2(xVelocity, yVelocity);
    rb.velocity = movement;
  }

  private bool isGrounded() {
    return groundCheck.GetComponent<GroundCheck>().isGrounded;
  }
}
