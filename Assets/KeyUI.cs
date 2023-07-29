using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour {

  [SerializeField] Image orangeUI;
  [SerializeField] Image purpleUI;
  [SerializeField] Image greenUI;

  private void Awake() {
    Key.ColorChanged += HandleKeyChange;
  }

  private void OnDestroy() {
    Key.ColorChanged -= HandleKeyChange;
  }

  private void HandleKeyChange(Key.KeyColor color) {
    switch (color) {
      case Key.KeyColor.Orange:
        orangeUI.color = getFullAlpha(orangeUI.color);
        break;
      case Key.KeyColor.Purple:
        purpleUI.color = getFullAlpha(purpleUI.color);
        break;
      case Key.KeyColor.Green:
        greenUI.color = getFullAlpha(greenUI.color);
        break;
      default:
        Debug.LogError("error");
        break;
    }
  }

  private Color getFullAlpha(Color color) {
    return new Color(color.r, color.g, color.b, 1f);
  }

}
