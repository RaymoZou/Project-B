using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {
  [SerializeField] Sprite emptySprite;
  [SerializeField] Sprite solidSprite;
  [SerializeField] bool isOpen;
  private SpriteRenderer spriteRenderer;
  private BoxCollider2D boxCollider;

  private Key.KeyColor colorCode = Key.KeyColor.Orange;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    boxCollider = GetComponent<BoxCollider2D>();
    Switch.OnInteract += HandleTriggered;
  }

  private void Start() {
    spriteRenderer.sprite = isOpen ? emptySprite : solidSprite;
  }

  private void OnDestroy() {
    Switch.OnInteract -= HandleTriggered;
  }

  private void HandleTriggered(Key.KeyColor color) {
    if (colorCode != color) return;
    isOpen = !isOpen;
    if (isOpen) {
      spriteRenderer.sprite = emptySprite;
      boxCollider.enabled = false;
    } else {
      spriteRenderer.sprite = solidSprite;
      boxCollider.enabled = true;
    }
  }
}
