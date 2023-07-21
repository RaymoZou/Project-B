using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

  public enum KeyColor { Orange, Purple, Green }
  public static event Action<KeyColor> ColorChanged;

  [SerializeField] private KeyColor color = KeyColor.Orange;

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.tag == "Player") {
      ColorChanged?.Invoke(color);
      Destroy(gameObject);
    }
  }
}
