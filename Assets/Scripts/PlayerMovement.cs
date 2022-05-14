using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

  Rigidbody2D rb;
  Animator myAnimator;
  SpriteRenderer spriteRenderer;

  [SerializeField] float moveSpeed = 3;
  [SerializeField] float jumpForce = 5;
  [SerializeField] GameObject groundCheck;


  float xInput;
  bool isJump = false;

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
    if (Input.GetButtonDown("Jump") && isGrounded()) {
      isJump = true;
      myAnimator.SetBool("isJump", isJump);
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
    float yVelocity = rb.velocity.y;
    if (isJump) yVelocity += jumpForce;
    rb.velocity = new Vector2(xInput * moveSpeed, yVelocity);
    isJump = false;
    myAnimator.SetBool("isJump", !isGrounded());
  }

  private bool isGrounded() {
    return groundCheck.GetComponent<GroundCheck>().isGrounded;
  }
}
