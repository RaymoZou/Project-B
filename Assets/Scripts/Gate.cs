using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

  private Key.KeyColor colorCode = Key.KeyColor.Orange;

  private void Awake() {
    Switch.OnInteract += HandleTriggered;
  }

  private void HandleTriggered(Key.KeyColor color) {
    if (colorCode != color) return;
    gameObject.SetActive(!isActiveAndEnabled);
  }
}
