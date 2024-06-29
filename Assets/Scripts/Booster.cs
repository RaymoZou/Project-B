using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour {

  [SerializeField] float boosterForce = 8.0f;


  // player must have RigidBody2D component
  private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.CompareTag("Player")) {
      Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
      rb.AddForce(transform.up * boosterForce, ForceMode2D.Impulse);
      //rb.velocity += Vector2.right * boosterForce;
    }
  }
}
