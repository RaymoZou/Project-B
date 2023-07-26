using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {

  [SerializeField] float damageTimer = 1f;
  [SerializeField] int damage = 25;
  public float currDamageTimer;

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.tag != "Player") return;
    Health playerHealth = collision.GetComponent<Health>();
    doDamage(playerHealth, damage);
    currDamageTimer = damageTimer;
  }

  private void OnTriggerStay2D(Collider2D collision) {

    if (collision.tag != "Player") return;
    Health playerHealth = collision.GetComponent<Health>();
    currDamageTimer -= Time.deltaTime;
    if (currDamageTimer < 0) {
      doDamage(playerHealth, damage);
      currDamageTimer = damageTimer;
    }
  }

  private void doDamage(Health health, int dmg) {
    health.DamagePlayer(dmg);
  }
}
