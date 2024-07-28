using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallCatch : MonoBehaviour {

  const int DAMAGE = 200;

  private void OnTriggerEnter2D(Collider2D other) {
    Health health = other.GetComponent<Health>();
    if (health) {
      health.DamagePlayer(DAMAGE);
    }
  }
}
