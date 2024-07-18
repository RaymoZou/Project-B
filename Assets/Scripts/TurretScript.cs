using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {
  [SerializeField] GameObject Bullet;
  const float SHOOT_OFFSET_X = -0.0339f;
  const float SHOOT_OFFSET_Y = 0.071f;
  Vector2 shootPoint;
  const float FIRE_RATE = 0.2f;
  const float FORCE = 50;
  float fireTime = 0; // time until the next bullet is fired

  private void Awake() {
    shootPoint = new(transform.position.x + SHOOT_OFFSET_X, transform.position.y + SHOOT_OFFSET_Y);
  }

  // Update is called once per frame
  void Update() {
    if (Time.time > fireTime) {
      fireTime = Time.time + 1 / FIRE_RATE;
      Shoot();
    }
  }

  void Shoot() {
    GameObject bullet = Instantiate(Bullet, shootPoint, Quaternion.identity);
    bullet.transform.localScale *= transform.localScale.x;
    bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.left * transform.localScale.x * FORCE);
  }
}
