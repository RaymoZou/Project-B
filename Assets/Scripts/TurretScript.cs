using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public GameObject Bullet;
    public Transform ShootPoint;
    public float FireRate;
    public float Force;
    float nextTimeToFire = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1 / FireRate;
            shoot();
        }
    }

    void shoot()
    {
        GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, Quaternion.identity);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(Vector2.left * Force);
    }
}
