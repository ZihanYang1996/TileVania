using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float initialBulletSpeed = 10f;
    [SerializeField] float bulletSpeedYMax = 10f;
    
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
    }

    void Update()
    {
        myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, bulletSpeedYMax);
    }

}
