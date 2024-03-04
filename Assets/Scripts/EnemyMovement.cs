using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = -1f;
    Rigidbody2D myRigidbody;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other) {
        // if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        // {
        //     Debug.Log("Triggered");
        // }
        moveSpeed *= -1;
        transform.localScale = new Vector3(-Mathf.Sign(moveSpeed), 1f, 1f);  // Flip the enemy sprite, the sign of the moveSpeed is the opposite of the localScale.x
    }
}
