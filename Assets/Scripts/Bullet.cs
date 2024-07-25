using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IDamageable {

  const int DAMAGE = 200;
  const float DESPAWN_TIMER = 10f;

  private void Start() {
    Invoke("Despawn", DESPAWN_TIMER);
  }

  void Despawn() { Destroy(gameObject); }

  // bullets can collide with tiles or the player
  private void OnCollisionEnter2D(Collision2D collision) {
    Health health = collision.gameObject.GetComponent<Health>();
    if (health) { // collision has health component
      DoDamage(health);
    } else {
      Destroy(gameObject);
    }
  }

  public void DoDamage(Health healthObject) {
    healthObject.DamagePlayer(DAMAGE);
  }
}
