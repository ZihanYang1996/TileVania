using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float maxSpeed = 15f;
    private float gravityScaleAtStart;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

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

        Run();
        ClimbLadder();
        InAirAnimation();
        FlipSprite();
        ClampVelocity();
    }

    void OnMove(InputValue value)
    {
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
}
