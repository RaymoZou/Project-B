using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

  protected bool isInteractable;

  public abstract void Interact();

  private void OnTriggerStay2D(Collider2D collision) {
    if (collision.CompareTag("Player")) {
      isInteractable = true;
    } else {
      isInteractable = false;
    }
  }

}
