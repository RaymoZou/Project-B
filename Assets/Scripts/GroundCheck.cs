using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

  public bool isGrounded;
  private float minDistanceOffGround = 0.1f;

  // casts a ray in the downward direction, if the object hit is the Ground layer then isGrounded is true otherwise it is false
  private void Update() {
    isGrounded = Physics2D.Raycast(transform.position, Vector2.down, minDistanceOffGround, LayerMask.GetMask("Ground"));
  }
}
