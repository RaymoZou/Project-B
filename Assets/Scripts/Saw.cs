using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour, IDamageable {

  [SerializeField] float speed = 2f;
  [SerializeField] float xRange = 3f;
  [SerializeField] int damage = 100;

  private Vector2 startPosition;
  private Vector2 endPosition;
  private Vector2 targetPosition;

  private void Start() {
    startPosition = transform.position;
    endPosition = new Vector2(transform.position.x + xRange, transform.position.y);
    targetPosition = endPosition;
  }

  private void FixedUpdate() {
    if (transform.position.x >= endPosition.x) targetPosition = startPosition;
    if (transform.position.x <= startPosition.x) targetPosition = endPosition;
    transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
  }

  private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.CompareTag("Player")) {
      DoDamage(collision.gameObject.GetComponent<Health>());
    }
  }

  public void DoDamage(Health health) {
    health.DamagePlayer(damage);
  }

}
