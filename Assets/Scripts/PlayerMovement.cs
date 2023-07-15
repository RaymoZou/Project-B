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
  [SerializeField] float accelFactor = 1f;
  [SerializeField] float topSpeed = 5f;
  [SerializeField] float accelerationRate = 15f;
  [SerializeField] float deaccelerationRate = 10f;
  [SerializeField] float jumpForce = 4f;
  [SerializeField] float maxJumpTime = 0.25f;
  [SerializeField] float gravityMultiplier = 1.5f;
  [SerializeField] float gravityScale = 1f;
  [SerializeField] float airBourneAccelerationRate = 5f;
  [SerializeField] float airBourneDeaccelerationRate = 20f;
  //[SerializeField] float jumpCutMultiplier = 0.5f;
  [SerializeField] float downwardForce = 1f;
  [SerializeField] float velPower = 1.25f;



  [SerializeField] float baseMoveSpeed = 2.0f;
  [SerializeField] float maxFallSpeed = 12.0f;


  private float xInput;
  private bool isJumping = false;
  private float currJumpTime;

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

    if (Input.GetButtonUp("Jump") && currJumpTime > 0.01f) {
      Vector2 tempDownward = Vector2.down * downwardForce * (currJumpTime / maxJumpTime);
      //Debug.Log(Mathf.Abs(tempDownward.y));
      rb.AddForce(tempDownward, ForceMode2D.Impulse);
    }

    #region Animation / Visuals
    if (myAnimator == null) return;
    myAnimator.SetBool("isJump", !isGrounded());
    myAnimator.SetBool("isWalking", xInput != 0 ? true : false);
    if (xInput == 1) spriteRenderer.flipX = false;
    if (xInput == -1) spriteRenderer.flipX = true;
    #endregion
  }

  // FixedUpdate is called once every 0.02 seconds or 50 times/second by default
  private void FixedUpdate() {
    #region Horizontal Movement
    //float xVelocity = rb.velocity.x + xInput * accelFactor * Time.deltaTime;
    //rb.AddForce(new Vector2(xInput * baseMoveSpeed, 0f));
    //rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -topSpeed, topSpeed), rb.velocity.y);
    //float proportionFriction = Mathf.Abs(rb.velocity.x) / topSpeed;
    //rb.velocity *= 1f - proportionFriction * Time.deltaTime;

    float targetVelocity = xInput * topSpeed;
    float speedDiff = targetVelocity - rb.velocity.x;
    float accelRate = (Mathf.Abs(targetVelocity) > 0.01f) ? accelerationRate : deaccelerationRate;
    if (!isGrounded()) accelRate = airBourneAccelerationRate;
    float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);
    if (isGrounded() || xInput != 0) rb.AddForce(movement * Vector2.right);
    //rb.AddForce(movement * Vector2.right);
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
