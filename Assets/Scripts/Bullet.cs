using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float initialBulletSpeed = 10f;
    // [SerializeField] float bulletSpeedYMax = 10f;
    [SerializeField] float bulletSpeedMax= 10f;
    [SerializeField] float bulletLifespan = 5f;

    [SerializeField] float maxDistance = 30f; // Roughly calculated diagonal of the screen

    float bulletSpeed;


    Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        // Get the mouse position in the world
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = 0;  // Ensure it's at the 0 z plane
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0;   // Ensure it's at the 0 z plane

        // Calculate the direction of the bullet
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;

        // Calculate the speed of the bullet
        float distance = Vector2.Distance(transform.position, mouseWorldPosition);
        bulletSpeed = Mathf.Lerp(0, bulletSpeedMax, Mathf.Clamp01(distance / maxDistance));  // Linear interpolation

        // Set the velocity of the bullet
        myRigidbody.velocity = direction * bulletSpeed;
        Destroy(gameObject, bulletLifespan);
    }

    void Update()
    {
        // Not needed, bullet will be destroyed after 5 seconds and Linear Drag is set to > 0
        // myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, bulletSpeedYMax);
    }

    // OnCollisionEnter2D is enough
    // When colliding with the box collider of the enemy, which is a trigger
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.tag == "Enemy")  // Can use other.gameObject.layer == LayerMask.GetMask("Enemy") instead of tag
    //     {
    //         Destroy(other.gameObject);
    //         Destroy(gameObject);
    //     }
    // }

    // When colliding with the capsule collider of the enemy
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")  // Can use other.gameObject.layer == LayerMask.GetMask("Enemy") instead of tag
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

}
