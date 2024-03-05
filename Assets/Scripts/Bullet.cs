using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float initialBulletSpeed = 10f;
    [SerializeField] float bulletSpeedYMax = 10f;
    [SerializeField] float bulletLifespan = 5f;
    
    float bulletSpeedX;


    Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = 0;  // Ensure it's at the 0 z plane
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0;   // Ensure it's at the 0 z plane
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        myRigidbody.velocity = direction * initialBulletSpeed;
        Destroy(gameObject, bulletLifespan);
    }

    void Update()
    {
        myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, bulletSpeedYMax);
    }

    // When colliding with the box collider of the enemy, which is a trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")  // Can use other.gameObject.layer == LayerMask.GetMask("Enemy") instead of tag
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
    
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
