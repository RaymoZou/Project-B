using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
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
    healthBar.maxValue = Health.defaultHealth;
    healthBar.value = Health.defaultHealth;
  }

  public void SetHealth(int hp) {
    healthBar.value = hp;
  }
}
