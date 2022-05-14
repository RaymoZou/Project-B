using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

  public bool isGrounded;

  private void OnTriggerEnter2D(Collider2D collision) {
    isGrounded = LayerMask.LayerToName(collision.gameObject.layer) == "Ground";
  }

  private void OnTriggerExit2D(Collider2D collision) {
    isGrounded = false;
  }

}
