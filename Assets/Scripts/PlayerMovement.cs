using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float maxSpeed = 15f;
    Vector2 moveInput;
    private float gravityScaleAtStart;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    bool isAlive = true;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }
    void Start()
    {

    }

    void Update()
    {
        if (!isAlive) return;
        Run();
        ClimbLadder();
        InAirAnimation();
        FlipSprite();
        ClampVelocity();
        Die(); // Can be replaced with OnCollisionEnter2D
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        // If the player is moving and touching the ground, then the player is running
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        bool playerIsTouchingGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed && playerIsTouchingGround);
    }

    void InAirAnimation()
    {
        bool onGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool isClimbing = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        myAnimator.SetBool("isJumping", !onGround && !isClimbing);
    }

    void OnJump(InputValue value)
    {
        // If the player is not touching the ground, then the player cannot jump
        if (!isAlive) return;
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void FlipSprite()
    {
        // Need to check if the player is moving, otherwise the player will always face right, since Mathf.Sign(0) = 1
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector3(Mathf.Sign(myRigidbody.velocity.x), 1f, 1f);
        }
    }

    void ClimbLadder()
    {
        // If the player is not touching the ladder, then the player cannot climb
        bool playerIsTouchingLadder = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (!playerIsTouchingLadder)
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            return;
        }

        myRigidbody.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * moveSpeed);
        myRigidbody.velocity = climbVelocity;
        if (moveInput.y != 0)
        {
            myAnimator.SetBool("isClimbing", true);
        }
        else
        {
            myAnimator.SetBool("isClimbing", false);
        }
    }

    void ClampVelocity()
    {
        // Clamp the velocity of the player so that the player does not move too fast
        
        // Debug.Log("Before Clamp: " + myRigidbody.velocity.magnitude);

        // This is the normal way to clamp the velocity
        myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity, maxSpeed);
        
        // Alternative way to clamp the velocity
        // if (myRigidbody.velocity.sqrMagnitude > (maxSpeed * maxSpeed))
        // {
        //     myRigidbody.velocity = myRigidbody.velocity.normalized * maxSpeed;
        // }

        // Debug.Log("Expected: " + Vector2.ClampMagnitude(myRigidbody.velocity, maxSpeed));
        // Debug.Log("After Clamp: " + myRigidbody.velocity.magnitude);
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = new Vector2(0f, jumpSpeed);
        }
    }

    // void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //     {
    //         isAlive = false;
    //         myAnimator.SetTrigger("Dying");
    //         myRigidbody.velocity = new Vector2(0f, jumpSpeed);
    //     }
    // }

    void OnFire(InputValue value)
    {
        if (!isAlive) return;
        myAnimator.SetTrigger("isAttacking");
    }
}
