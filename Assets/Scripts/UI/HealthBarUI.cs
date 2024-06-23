using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {
  public Slider healthBar;

  private void Awake() {
    healthBar = GetComponent<Slider>();
    Health.OnHealthChanged += SetHealth;
  }

  private void OnDestroy() {
    Health.OnHealthChanged -= SetHealth;
  }

  // Start is called before the first frame update
  void Start() {
    healthBar.maxValue = Health.HEALTH;
    healthBar.value = Health.HEALTH;
  }

  public void SetHealth(int hp, int playerLayer) {
    if (playerLayer != gameObject.layer) return;
    healthBar.value = hp;
  }
}
