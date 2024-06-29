using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour {

  [SerializeField] SpriteRenderer activatedCheckpoint;
  public static event Action<Vector2, int> OnActivate;

  private void OnTriggerEnter2D(Collider2D collision) {
    if (!collision.CompareTag("Player")) return; // if it's not a player then return
    if (collision.gameObject.layer == gameObject.layer) {
      activatedCheckpoint.gameObject.SetActive(true);
      OnActivate?.Invoke(new Vector2(transform.position.x, transform.position.y), gameObject.layer);
    }
  }

}
