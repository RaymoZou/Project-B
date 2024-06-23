using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
  public const int HEALTH = 100; // starting health of the player
  [SerializeField] private float DAMAGE_COOLDOWN = 0.5f; // amount of time before player can be damaged again
  private Color PLAYER_COLOR;
  public static int currHealth = HEALTH;
  private float currCooldown = 0;
  private bool isOnCooldown;
  public static event Action<int, int> OnHealthChanged;
  public static event Action<GameObject> OnDeath;
  public static event Action<Transform> OnSpawn;

  // Start is called before the first frame update
  void Start()
  {
    currHealth = HEALTH;
    PLAYER_COLOR = GetComponent<SpriteRenderer>().color;
    OnHealthChanged?.Invoke(currHealth, gameObject.layer);
    OnSpawn?.Invoke(transform);
  }

  private void Update()
  {
    currCooldown -= Time.deltaTime;
    isOnCooldown = currCooldown < 0 ? false : true;
    if (isOnCooldown)
    {
      GetComponent<SpriteRenderer>().color = new Color(PLAYER_COLOR.r, PLAYER_COLOR.g, PLAYER_COLOR.b, 0.5f);
    } else
    {
      GetComponent<SpriteRenderer>().color = PLAYER_COLOR;
    }
  }

  public void DamagePlayer(int damage)
  {
    // take damage if not on cooldown
    if (currCooldown < 0f)
    {
      currCooldown = DAMAGE_COOLDOWN;
      currHealth -= damage;
      OnHealthChanged?.Invoke(currHealth, gameObject.layer);
    }
    if (currHealth <= 0f)
    {
      OnDeath?.Invoke(gameObject);
    }
  }
}
