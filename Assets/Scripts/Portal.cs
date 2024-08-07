using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;

public class Portal : MonoBehaviour {
  public static event Action Finish;

  private void OnTriggerEnter2D(Collider2D collision) {
    // TODO: remove player check (handled by physics layers);
    if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
      Finish?.Invoke();
    }
  }
}
