using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

  protected bool isInteractable;
  protected GameObject player;
  public abstract void Interact();

  // TODO: refactor player check
  protected virtual void OnTriggerStay2D(Collider2D collision) {
    if (collision.CompareTag("Player 1") || collision.CompareTag("Player 2")) {
      isInteractable = true;
      player = collision.gameObject;
      collision.gameObject.GetComponent<PlayerController>().SetInteractable(this);
    }
  }

  protected virtual void OnTriggerExit2D(Collider2D collision) {
    if (collision.CompareTag("Player 1") || collision.CompareTag("Player 2")) {
      isInteractable = false;
      player = null;
      collision.gameObject.GetComponent<PlayerController>().SetInteractable(null);
    }
  }

}
