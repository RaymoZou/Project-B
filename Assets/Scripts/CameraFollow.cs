using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

  public static Transform player;

  public float followSpeed = 1.5f;

  public static void ResetPlayerTransform(Transform newTransform) {
    player = newTransform;
  }

  void Update() {
    if (player == null) return;
    Vector3 targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
  }
}
