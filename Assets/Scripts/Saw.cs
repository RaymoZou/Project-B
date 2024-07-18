using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour, IDamageable {

  [SerializeField] float SPEED = 2f;
  [SerializeField] float RANGE = 3f; // how far from the initial position the saw will move
  [SerializeField] int DAMAGE = 100;

  private Vector2 startPosition;
  private Vector2 endPosition;
  private Vector2 targetPosition;

  private void Start() {
    startPosition = new Vector2(transform.position.x - RANGE / 2, transform.position.y);
    endPosition = new Vector2(transform.position.x + RANGE / 2, transform.position.y);
    targetPosition = endPosition;
  }

  private void FixedUpdate() {
    if (transform.position.x >= endPosition.x) targetPosition = startPosition;
    if (transform.position.x <= startPosition.x) targetPosition = endPosition;
    transform.position = Vector2.MoveTowards(transform.position, targetPosition, SPEED * Time.deltaTime);
  }

  private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.CompareTag("Player")) {
      DoDamage(collision.gameObject.GetComponent<Health>());
    }
  }

  public void DoDamage(Health health) {
    health.DamagePlayer(DAMAGE);
  }

}
