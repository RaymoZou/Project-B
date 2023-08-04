using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour, IDamageable {

  [SerializeField] int damage = 25;
  [SerializeField] float despawnTimer = 10f;

  private void Start() {
    StartCoroutine(DespawnCoroutine());
  }

  IEnumerator DespawnCoroutine() {
    yield return new WaitForSeconds(despawnTimer);
    Destroy(gameObject);
  }


  private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.tag == "Player") {
      Health playerHealth = collision.gameObject.GetComponent<Health>();
      DoDamage(playerHealth);
    }
    Destroy(gameObject);
  }

  public void DoDamage(Health healthObject) {
    healthObject.DamagePlayer(damage);
  }
}
