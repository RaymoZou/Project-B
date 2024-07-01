using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Light2D))]

public class Spike : MonoBehaviour {

  [SerializeField] int damage = 25;
  new PolygonCollider2D collider;
  private void Awake() {
    collider = GetComponent<PolygonCollider2D>();
    collider.isTrigger = true;
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.tag != "Player") return;
    Health playerHealth = collision.GetComponent<Health>();
    doDamage(playerHealth, damage);
  }

  private void OnTriggerStay2D(Collider2D collision) {

    if (collision.tag != "Player") return;
    Health playerHealth = collision.GetComponent<Health>();
    doDamage(playerHealth, damage);
  }

  private void doDamage(Health health, int dmg) {
    health.DamagePlayer(dmg);
  }
}
