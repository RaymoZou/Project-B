using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
  public int curHealth = 0;
  public static int defaultHealth = 100;
  public static event Action<int> OnHealthChanged;
  public static event Action<GameObject> OnDeath;

  private void Awake() {
    CameraFollow.ResetPlayerTransform(transform);
  }

  // Start is called before the first frame update
  void Start() {
    curHealth = defaultHealth;
    OnHealthChanged?.Invoke(curHealth);
  }

  public void DamagePlayer(int damage) {
    curHealth -= damage;
    OnHealthChanged?.Invoke(curHealth);
    if (curHealth <= 0) {
      OnDeath?.Invoke(gameObject);
    }
  }
}
