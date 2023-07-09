using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

  Rigidbody2D rb;
  Animator myAnimator;
  SpriteRenderer spriteRenderer;

  [SerializeField] float moveSpeed = 3;
  [SerializeField] float jumpForce = 4;
  [SerializeField] GameObject groundCheck;


  private float xInput;
  public bool isJumping = false;
  private float jumpTime = 0.35f;
  public float currJumpTimer;

  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    myAnimator = GetComponent<Animator>();
  }

  // Start is called before the first frame update
  void Start() {
  }

  // Update is called once per frame
  void Update() {
    xInput = Input.GetAxisRaw("Horizontal");
    // have to be grounded and not already in a jump
    if (Input.GetButtonDown("Jump") && isGrounded() && !isJumping) {
      isJumping = true;
      currJumpTimer = jumpTime;
      myAnimator.SetBool("isJump", isJumping);
    }
    if (xInput != 0) {
      myAnimator.SetBool("isWalking", true);
    } else {
      myAnimator.SetBool("isWalking", false);
    }

    // set sprite flip orientation
    if (xInput == 1) spriteRenderer.flipX = false;
    if (xInput == -1) spriteRenderer.flipX = true;
  }

  private void FixedUpdate() {
    if (isJumping) {
      if (Input.GetButton("Jump") && currJumpTimer > 0) {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        currJumpTimer -= Time.deltaTime;
      } else {
        isJumping = false;
      }
    }
    rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    myAnimator.SetBool("isJump", !isGrounded());
  }

  private bool isGrounded() {
    return groundCheck.GetComponent<GroundCheck>().isGrounded;
  }
}
