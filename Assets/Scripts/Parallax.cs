using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

  // TODO: reimplement parallax that takes into account the two cameras
  [SerializeField] Camera cam;
  [SerializeField] float followSpeed = 0.5f;

  private void Update() {
    transform.position = new Vector3(cam.transform.position.x * followSpeed, cam.transform.position.y * followSpeed, transform.position.z);
  }
}
