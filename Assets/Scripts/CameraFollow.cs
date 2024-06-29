using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

  private Transform playerTransform;
  readonly float followSpeed = 1.5f;

  private void Awake() {
    // allocate screen space
    if (gameObject.layer == LayerMask.NameToLayer("Player 1")) {
      GetComponent<Camera>().rect = new Rect(0f, 0f, 0.5f, 1f);
    } else if (gameObject.layer == LayerMask.NameToLayer("Player 2")) {
      GetComponent<Camera>().rect = new Rect(0.5f, 0f, 0.5f, 1f);
    }
    Health.OnSpawn += SetPlayerTransform; // reset the playerTransform when the player spawns
  }

  // set the player transform the camera will be following
  private void SetPlayerTransform(Transform transform, int playerLayer) {
    if (playerLayer == gameObject.layer) {
      playerTransform = transform;
    }
  }

  // update the assigned player transform every frame
  void Update() {
    if (playerTransform == null) return;
    Vector3 targetPos = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
  }
}
