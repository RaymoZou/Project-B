using System;
using UnityEngine;

public class StartLine : MonoBehaviour {
  public static event Action OnStart;
  private void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      OnStart?.Invoke();
    }
  }
}
