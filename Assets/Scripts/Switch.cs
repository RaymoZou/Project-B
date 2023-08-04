using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactable {

  [SerializeField] Key.KeyColor colorCode;
  private SpriteRenderer spriteRenderer;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  public static event Action<Key.KeyColor> OnInteract;

  public override void Interact() {
    if (!isInteractable) return;
    spriteRenderer.flipX = !spriteRenderer.flipX;
    OnInteract?.Invoke(colorCode);
  }
}
