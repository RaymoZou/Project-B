using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour {

  [SerializeField] SpriteRenderer activatedCheckpoint;
  public static event Action<Vector2, int> OnActivate;

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.gameObject.tag == gameObject.tag) {
      activatedCheckpoint.gameObject.SetActive(true);
      OnActivate?.Invoke(new Vector2(transform.position.x, transform.position.y), gameObject.layer);
    }
  }

}
