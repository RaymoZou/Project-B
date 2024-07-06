using System.Collections;
using UnityEngine;

// the camera's default z-layer = -1
public class CameraFollow : MonoBehaviour {

  private Transform playerTransform;
  const float FOLLOW_SPEED = 1.5f;
  const float SHAKE_AMP = 0.2f;
  const float SHAKE_DURATION = 0.3f;
  public float currShakeDuration;
  private static int numCameras = 0;

  // camera shake when the player dies
  private IEnumerator Shake() {
    Vector2 initialPos = transform.localPosition;
    while (currShakeDuration < SHAKE_DURATION) {
      transform.localPosition = initialPos + Random.insideUnitCircle * SHAKE_AMP;
      transform.localPosition = new(transform.localPosition.x, transform.localPosition.y, -1);
      currShakeDuration += Time.deltaTime;
      yield return null;
    }
    currShakeDuration = 0;
  }

  private void StartShake(GameObject player) {
    if (player.layer != gameObject.layer) return;
    StartCoroutine(Shake());
  }

  private void Awake() {
    numCameras++;
    Health.OnSpawn += SetPlayerTransform;
    Health.OnDeath += StartShake;
  }

  private void OnDestroy() {
    Health.OnSpawn -= SetPlayerTransform;
    Health.OnDeath -= StartShake;
  }

  private void Start() {
    if (numCameras == 1) {
      GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 1f);
    } else if (numCameras == 2) {
      if (gameObject.layer == LayerMask.NameToLayer("Player 1")) {
        GetComponent<Camera>().rect = new Rect(0f, 0f, 0.5f, 1f);
      } else if (gameObject.layer == LayerMask.NameToLayer("Player 2")) {
        GetComponent<Camera>().rect = new Rect(0.5f, 0f, 0.5f, 1f);
      }
    }
  }

  // set the player Transform the camera will be following
  private void SetPlayerTransform(Transform transform, int playerLayer) {
    if (playerLayer == gameObject.layer) {
      playerTransform = transform;
    }
  }

  // update the assigned player transform every frame
  void Update() {
    if (playerTransform == null) return;
    Vector3 targetPos = new(playerTransform.position.x, playerTransform.position.y, -1);
    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * FOLLOW_SPEED);
  }
}
