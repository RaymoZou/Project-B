using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour {

  public bool isEnabled = false;
  private Image image;
  private Animator animator;


  private void Awake() {
    image = GetComponent<Image>();
    PlayerMovement.OnDashChange += SetDisabled;
    animator = GetComponent<Animator>();
  }

  private void OnDestroy() {
    PlayerMovement.OnDashChange -= SetDisabled;
  }

  private void SetDisabled(float disableTime) {
    StartCoroutine(SetDisabledCouroutine(disableTime));
  }

  IEnumerator SetDisabledCouroutine(float disableTime) {
    AnimationClip clip = animator.runtimeAnimatorController.animationClips[0];
    animator.Play(clip.name, -1, 0f);
    animator.speed = 0f;
    yield return null;
    animator.speed = clip.length / disableTime;

    Color tempColor = image.color;
    tempColor.a = 0.5f;
    image.color = tempColor;
    yield return new WaitForSeconds(disableTime);
    tempColor.a = 1.0f;
    image.color = tempColor;
  }

}
