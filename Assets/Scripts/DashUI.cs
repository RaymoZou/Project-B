using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour {

  public bool isEnabled = false;
  private Image image;

  private void Awake() {
    image = GetComponent<Image>();
    PlayerMovement.OnDashChange += SetDisabled;
  }

  private void OnDestroy() {
    PlayerMovement.OnDashChange -= SetDisabled;
  }

  private void SetDisabled(float disableTime) {
    StartCoroutine(SetDisabledCouroutine(disableTime));
  }

  IEnumerator SetDisabledCouroutine(float disableTime) {
    Color tempColor = image.color;
    tempColor.a = 0.5f;
    image.color = tempColor;
    yield return new WaitForSeconds(disableTime);
    tempColor.a = 1.0f;
    image.color = tempColor;
  }

}
