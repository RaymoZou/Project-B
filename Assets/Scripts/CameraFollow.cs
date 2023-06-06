using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

  [SerializeField] Transform player;

  public float followSpeed = 1.5f;

  // Update is called once per frame
  void Update() {
    // slerp x
    Vector3 targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
  }
}
