using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallCatch : MonoBehaviour, IDamageable {

  private int damage = 200;

  public void DoDamage(Health health) {
    health.DamagePlayer(damage);
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.tag != "Player") return;
    Health playerHealth = other.GetComponent<Health>();
    DoDamage(playerHealth);
  }
}
