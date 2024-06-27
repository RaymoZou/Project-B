using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {
  [SerializeField] GameObject Bullet;
  // TODO: make this a constant offset - remove need for a Transform that
  // needs to be configured in the Inspector
  public Transform ShootPoint;
  private float FIRE_RATE = 0.75f;
  private float FORCE = 50;
  float nextTimeToFire = 0;

  // Update is called once per frame
  void Update() {
    if (Time.time > nextTimeToFire) {
      nextTimeToFire = Time.time + 1 / FIRE_RATE;
      shoot();
    }
  }

  void shoot() {
    GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, Quaternion.identity);
    BulletIns.GetComponent<Rigidbody2D>().AddForce(Vector2.left * FORCE);
  }
}
