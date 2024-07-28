using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class TNT : MonoBehaviour {

  private SpriteRenderer spriteRenderer;
  [SerializeField] GameObject explosionIndicator;
  [SerializeField] SpriteRenderer cover;
  const float TIMER = 2f; // time until explosion upon player collision
  const float EXPLOSION_RADIUS = 3f; // explosion radius
  const float FLASH_INTERVAL = 0.1f;


  const float FLASH_ALPHA = 0.5f;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    if (!explosionIndicator) Debug.LogError("set the explosion indicator in the inspector!");
  }

  private void Start() {
    // multiply by 2 to convert from circle radius to square side length
    explosionIndicator.transform.localScale *= EXPLOSION_RADIUS * 2;
  }

  IEnumerator Flash() {
    cover.color = new Color(1, 1, 1, FLASH_ALPHA);
    yield return new WaitForSeconds(FLASH_INTERVAL); // flash interval
    cover.color = new Color(1, 1, 1, 1);
    yield return new WaitForSeconds(FLASH_INTERVAL); // flash interval
    cover.color = new Color(1, 1, 1, FLASH_ALPHA);
  }

  IEnumerator Explode() {
    yield return new WaitForSeconds(TIMER - 0.5f);
    cover.color = Color.white;
    explosionIndicator.SetActive(true);
    yield return new WaitForSeconds(0.25f);
    Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, EXPLOSION_RADIUS, LayerMask.GetMask("Player"));
    foreach (Collider2D player in players) {
      Debug.Log(player.name);
      Health health = player.GetComponent<Health>();
      if (health) {
        health.DamagePlayer(300);
      }
    }
    Destroy(gameObject);
  }

  // TODO: remove player check - handled by physics layer
  private void OnCollisionEnter2D(Collision2D other) {
    if (other.gameObject.tag == "Player 1" || other.gameObject.tag == "Player 2") {
      StartCoroutine(Flash());
      StartCoroutine(Explode());
    }
  }
}
