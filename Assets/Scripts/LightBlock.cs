using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBlock : MonoBehaviour {

  [SerializeField] Sprite inactiveSprite;
  [SerializeField] Sprite activeSprite;

  [SerializeField] float INACTIVE_INTENSITY = 0.1f;
  [SerializeField] float ACTIVE_INTENSITY = 0.6f;

  [SerializeField] BoxCollider2D solidCollider;
  private CircleCollider2D bulletCollider;
  private SpriteRenderer spriteRenderer;
  private Light2D blockLight;
  private bool isActive;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    blockLight = GetComponent<Light2D>();
    bulletCollider = GetComponent<CircleCollider2D>();
  }

  private void Start() {
    blockLight.intensity = INACTIVE_INTENSITY;
  }

  private void OnTriggerStay2D(Collider2D other) {
    if (!other.GetComponent<BulletScript>()) return;
    spriteRenderer.sprite = activeSprite;
    solidCollider.enabled = true;
    blockLight.intensity = ACTIVE_INTENSITY;
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (!other.GetComponent<BulletScript>()) return;
    spriteRenderer.sprite = inactiveSprite;
    solidCollider.enabled = false;
    blockLight.intensity = INACTIVE_INTENSITY;
  }

}
