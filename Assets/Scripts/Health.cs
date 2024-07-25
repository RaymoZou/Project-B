using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
  public const int HEALTH = 100; // starting health of the player
  const float DAMAGE_COOLDOWN = 2.8f; // amount of time before player can be damaged again
  private Color PLAYER_COLOR;
  public static int currHealth = HEALTH;
  private float currCooldown = 0;
  private bool isOnCooldown;
  public static event Action<int, int> OnHealthChanged;
  public static event Action<GameObject> OnDeath;
  public static event Action<Transform, int> OnSpawn;
  SpriteRenderer spriteRenderer;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  void Start() {
    currHealth = HEALTH;
    PLAYER_COLOR = spriteRenderer.color;
    OnHealthChanged?.Invoke(currHealth, gameObject.layer);
    OnSpawn?.Invoke(transform, gameObject.layer);
  }

  private void Update() {
    currCooldown -= Time.deltaTime;
    isOnCooldown = currCooldown >= 0;
    if (isOnCooldown) {
      spriteRenderer.color = new Color(PLAYER_COLOR.r, PLAYER_COLOR.g, PLAYER_COLOR.b, 0.5f);
    } else {
      spriteRenderer.color = PLAYER_COLOR;
    }
  }

  public void DamagePlayer(int damage) {
    // take damage if not on cooldown and player is not already dead
    if (currHealth <= 0f) return;
    if (currCooldown < 0f) {
      currCooldown = DAMAGE_COOLDOWN;
      currHealth -= damage;
      OnHealthChanged?.Invoke(currHealth, gameObject.layer);
    }
    // die
    if (currHealth <= 0f) {
      OnDeath?.Invoke(gameObject);
    }
  }
}
