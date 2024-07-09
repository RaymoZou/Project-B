using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class RestartButton : Interactable {

  public override void Interact() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload the current scene
  }

  protected override void OnTriggerStay2D(Collider2D other) {
    base.OnTriggerStay2D(other);
    transform.GetChild(0).gameObject.SetActive(true); // set the first child inactive
  }
  protected override void OnTriggerExit2D(Collider2D other) {
    base.OnTriggerExit2D(other);
    transform.GetChild(0).gameObject.SetActive(false); // set the first child inactive
  }
}

