using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Portal : MonoBehaviour {

  private bool isOpen = false;
  [SerializeField] GameObject orangeKey;
  [SerializeField] GameObject purpleKey;
  [SerializeField] GameObject greenKey;

  private void Awake() {
    Key.ColorChanged += CollectKey;
  }

  private void CollectKey(Key.KeyColor color) {
    Debug.Log(color + " key has been collected.");
    //if (color == Key.KeyColor.Orange) orangeKey.SetActive(true);
    switch (color) {
      case Key.KeyColor.Orange:
        orangeKey.SetActive(true);
        break;
      case Key.KeyColor.Green:
        greenKey.SetActive(true);
        break;
      case Key.KeyColor.Purple:
        purpleKey.SetActive(true);
        break;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("Player") && isOpen) {
      GameManager.LoadNextLevel();
    }
  }
}
