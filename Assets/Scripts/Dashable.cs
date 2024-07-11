using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UIElements;

[RequireComponent(typeof(SpriteRenderer))]
public class Dashable : Interactable {
  SpriteRenderer spriteRenderer;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  public override void Interact() {
    // (player.transform.position, transform.position) = (transform.position, player.transform.position);
    Vector2 vector = transform.position - player.transform.position;
    player.transform.position = transform.position;
    Debug.Log(vector.normalized * 15);
    // player.GetComponent<Rigidbody2D>().AddForce(vector.normalized * 15, ForceMode2D.Impulse);
    player.GetComponent<Rigidbody2D>().velocity = new(10, 10);
  }

  protected override void OnTriggerStay2D(Collider2D other) {
    base.OnTriggerStay2D(other);
    spriteRenderer.color = new(255, 0, 0, 255);
  }

  protected override void OnTriggerExit2D(Collider2D other) {
    base.OnTriggerExit2D(other);
    spriteRenderer.color = new(255, 255, 255, 255);
  }

}
