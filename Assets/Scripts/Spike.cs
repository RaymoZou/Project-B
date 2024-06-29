using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {

  [SerializeField] int damage = 25;

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
