using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Light2D))]

public class Spike : MonoBehaviour {

  const int DAMAGE = 200;
  new PolygonCollider2D collider;
  private void Awake() {
    collider = GetComponent<PolygonCollider2D>();
    collider.isTrigger = true;
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    Health health = collision.GetComponent<Health>();
    if (health) {
      DoDamage(health, DAMAGE);
    }
  }

  private void DoDamage(Health health, int dmg) {
    health.DamagePlayer(dmg);
  }
}
