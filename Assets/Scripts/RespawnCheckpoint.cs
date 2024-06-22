using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour {

  [SerializeField] SpriteRenderer activatedCheckpoint;
  public static event Action<Vector2> OnActivate; 

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("Player")) {
      if (collision.gameObject.layer == gameObject.layer)
      {
        Debug.Log("it's a match!");
        activatedCheckpoint.gameObject.SetActive(true);
        OnActivate?.Invoke(new Vector3(transform.position.x, transform.position.y));
      }
    }
  }

}
