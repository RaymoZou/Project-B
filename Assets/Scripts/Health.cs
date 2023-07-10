using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
  public int curHealth = 0;
  public static int defaultHealth = 100;
  public static event Action<int> OnHealthChanged;

  // Start is called before the first frame update
  void Start() {
    curHealth = defaultHealth;
  }

  public void DamagePlayer(int damage) {
    curHealth -= damage;
    OnHealthChanged?.Invoke(curHealth);
  }
}
