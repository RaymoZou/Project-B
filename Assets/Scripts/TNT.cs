using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : MonoBehaviour {

  private SpriteRenderer spriteRenderer;
  [SerializeField] GameObject explosionIndicator;
  const float TIMER = 2f; // time until explosion upon player collision
  const float EXPLOSION_RADIUS = 3f; // explosion radius

  const int FLASH_ALPHA = 128;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    if (!explosionIndicator) Debug.LogError("set the explosion indicator in the inspector!");
  }

  private void Start() {
    // multiply by 2 to convert from circle radius to square side length
    explosionIndicator.transform.localScale *= EXPLOSION_RADIUS * 2;
  }

  // repeatedly change the sprite color
  // TODO: flash the TNT sprite
  void Flash() {
    if (spriteRenderer.color == Color.white) {
      // reduce alpha
      Debug.Log("other");
      spriteRenderer.color = new(255, 255, 255, FLASH_ALPHA);
    } else {
      // restore alpha
      Debug.Log("white");
      spriteRenderer.color = Color.white;
    }
  }

  IEnumerator Explode() {
    spriteRenderer.color = Color.red;
    explosionIndicator.SetActive(true);
    yield return new WaitForSeconds(TIMER);
    Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, EXPLOSION_RADIUS, LayerMask.GetMask("Player 1"));
    foreach (Collider2D player in players) {
      Debug.Log(player.name);
      Health health = player.GetComponent<Health>();
      if (health) {
        health.DamagePlayer(300);
      }
    }
    Destroy(gameObject);
  }

  private void OnCollisionEnter2D(Collision2D other) {
    if (other.gameObject.tag == "Player") {
      InvokeRepeating("Flash", 0.1f, 0.1f);
      StartCoroutine(Explode());
    }
  }
}
