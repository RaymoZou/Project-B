using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Portal : MonoBehaviour {

  private bool isOpen = false;
  [SerializeField] GameObject orangeKey;
  [SerializeField] GameObject purpleKey;
  [SerializeField] GameObject greenKey;
  [SerializeField] Sprite openSprite;

  private SpriteRenderer spriteRenderer;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    Key.ColorChanged += CollectKey;
  }

  private void CollectKey(Key.KeyColor color) {
    switch (color) {
      case Key.KeyColor.Orange:
        orangeKey.SetActive(true);
        break;
      case Key.KeyColor.Green:
        greenKey.SetActive(true);
        break;
      case Key.KeyColor.Purple:
        purpleKey.SetActive(true);
        break;
    }

    isOpen = orangeKey.activeSelf && greenKey.activeSelf && purpleKey.activeSelf;
    if (isOpen) spriteRenderer.sprite = openSprite;
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("Player") && isOpen) {
      GameManager.LoadNextLevel();
    }
  }
}
