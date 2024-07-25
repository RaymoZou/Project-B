using UnityEngine;
using UnityEngine.Rendering;

public class Boulder : MonoBehaviour {

  Rigidbody2D rb;
  private int DAMAGE = 300;
  private float MAX_SPEED = 7.5f;

  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
  }

  private void Update() {
    if (rb.velocity.y < -MAX_SPEED) rb.velocity = new(rb.velocity.x, -MAX_SPEED); // clamp fall speed
    if (transform.position.y < -50) Destroy(gameObject); // destory boulder if out of bounds
  }

  // TODO: bounce on the platforms
  private void OnCollisionEnter2D(Collision2D other) {
    Health playerHealth = other.gameObject.GetComponent<Health>();
    playerHealth.DamagePlayer(DAMAGE);
    rb.velocity = Vector2.zero;
  }
}
