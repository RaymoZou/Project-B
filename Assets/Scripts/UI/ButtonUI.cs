using UnityEngine;
using UnityEngine.UI;

//Use the Selectable class as a base class to access the IsHighlighted method
public class Example : MonoBehaviour {
  Button button;

  void Start() {
    button = GetComponent<Button>();
  }
  void Update() { }
}
