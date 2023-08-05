using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;

public class Portal : MonoBehaviour {

  private bool isOpen = false;
  [SerializeField] GameObject orangeKey;
  [SerializeField] GameObject purpleKey;
  [SerializeField] GameObject greenKey;
  [SerializeField] Sprite openSprite;

  [SerializeField] GameObject portalLight;

  private SpriteRenderer spriteRenderer;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    Key.ColorChanged += CollectKey;
  }

  private void OnDestroy() {
    Key.ColorChanged -= CollectKey;
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
    if (isOpen) {
      spriteRenderer.sprite = openSprite;
      portalLight.SetActive(true);
    }
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("Player") && isOpen) {
      GameManager.LoadNextLevel();
    }
  }
}
