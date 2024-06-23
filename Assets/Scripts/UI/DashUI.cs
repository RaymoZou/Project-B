using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour {

  public bool isEnabled = false;
  private Image image;
  private Animator animator;

  private AnimationClip loadAnimation;


  private void Awake() {
    image = GetComponent<Image>();
    PlayerController.OnDashChange += SetDisabled;
    animator = GetComponent<Animator>();
  }

  private void Start() {
    loadAnimation = animator.runtimeAnimatorController.animationClips[0];
  }

  private void OnDestroy() {
    PlayerController.OnDashChange -= SetDisabled;
  }

  private void SetDisabled(float disableTime, int playerLayer) {
    if (playerLayer != gameObject.layer) return;
    StartCoroutine(SetDisabledCouroutine(disableTime));
  }

  IEnumerator SetDisabledCouroutine(float disableTime) {
    animator.Play(loadAnimation.name, -1, 0f);
    animator.speed = loadAnimation.length / disableTime; 
    yield return null;
    Color originalColor = image.color;
    originalColor.a = 0.5f;
    image.color = originalColor;
    yield return new WaitForSeconds(disableTime);
    originalColor.a = 1.0f;
    image.color = originalColor;
  }

}
