using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

  public Transform playerTransform;
  public float followSpeed = 1.5f;

  private void Awake()
  {
    // allocate screen space
    if (gameObject.layer == LayerMask.NameToLayer("Player 1"))
    {
      GetComponent<Camera>().rect = new Rect(0f, 0f, 0.5f, 1f);
    } else if (gameObject.layer == LayerMask.NameToLayer("Player 2"))
    {
      GetComponent<Camera>().rect = new Rect(0.5f, 0f, 0.5f, 1f);
    }
    Health.OnSpawn += HandleEvent;
  }

  private void HandleEvent(Transform transform)
  {
    if (transform.gameObject.layer == gameObject.layer)
    {
      playerTransform = transform;
    }
  }

  public void ResetPlayerTransform(Transform newTransform)
  {
    playerTransform = newTransform;
  }

  void Update()
  {
    if (playerTransform == null) return;
    Vector3 targetPos = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
  }
}
