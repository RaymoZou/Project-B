using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

  protected bool isInteractable;

  public abstract void Interact();

  protected virtual void OnTriggerStay2D(Collider2D collision) {
    if (collision.CompareTag("Player")) {
      isInteractable = true;
      collision.gameObject.GetComponent<PlayerController>().SetInteractable(this);
    }
  }

  protected virtual void OnTriggerExit2D(Collider2D collision) {
    if (collision.CompareTag("Player")) {
      isInteractable = false;
      collision.gameObject.GetComponent<PlayerController>().SetInteractable(null);
    }
  }

}
