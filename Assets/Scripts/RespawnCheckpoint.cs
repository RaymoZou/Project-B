using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour {

  [SerializeField] Sprite activatedCheckpoint;
  SpriteRenderer spriteRenderer;
  public static event Action<Vector2> OnActivate; 

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("Player")) {
      spriteRenderer.sprite = activatedCheckpoint;
      OnActivate?.Invoke(new Vector3(transform.position.x, transform.position.y));
    }
  }

}
