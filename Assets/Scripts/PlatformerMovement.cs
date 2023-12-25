using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public BoxCollider2D coll; // assuming its a rectangular sprite

    private Vector2 thumbstick;
    private bool pressedJump;
    private Vector2 instantaneousVelocity;
    private float currTime = 0f;

    void Update()
    {
        thumbstick.x = Input.GetAxisRaw("Horizontal");
        thumbstick.y = Input.GetAxisRaw("Vertical");
        pressedJump = Input.GetButton("Jump");

        currTime += Time.deltaTime;
    }

    void FixedUpdate()
    {
        CheckCollisions();

        // Compute the instantaneous velocity
        PlayerJump();
        HorizontalVelocity();
        PlayerGravity();

        rb.velocity = instantaneousVelocity; // Apply the computed velocity
    }

    [Header("Collision Properties")]
    [SerializeField] public float detectionDistance;

    private void CheckCollisions()
    {
        bool stashStartInColliders = Physics2D.queriesStartInColliders;
        Physics2D.queriesStartInColliders = false;

        // Raycast up and down
        bool hitGround = Physics2D.BoxCast(coll.bounds.center,
                                            coll.size,
                                            0,
                                            Vector2.down,
                                            detectionDistance,
                                            ~LayerMask.GetMask("Player"));

        bool hitCeiling = Physics2D.BoxCast(coll.bounds.center,
                                            coll.size,
                                            0,
                                            Vector2.up,
                                            detectionDistance,
                                            ~LayerMask.GetMask("Player"));

        // Hit a ceiling
        if (hitCeiling) { instantaneousVelocity.y = Mathf.Min(0, instantaneousVelocity.y); }

        // Landed on Ground
        if (hitGround && !grounded) { grounded = true; }

        // Leaving the Ground
        if (!hitGround && grounded) { grounded = false; }

        Physics2D.queriesStartInColliders = stashStartInColliders;
    }

    [Header("Jumping Properties")]
    [SerializeField] public float jumpSpeed;
    private bool grounded;

    private void PlayerJump()
    {
        if (grounded && pressedJump) { instantaneousVelocity.y = jumpSpeed; }
    }


    [Header("Running Properties")]
    [SerializeField] public float runTopSpeed;
    [SerializeField] public float runAcceleration;
    [SerializeField] public float runDeceleration;

    private void HorizontalVelocity()
    {
        if (thumbstick.x == 0)
        {
            instantaneousVelocity.x = Mathf.MoveTowards(instantaneousVelocity.x,
                                                        0,
                                                        runDeceleration * Time.fixedDeltaTime);
        }
        else
        {
            instantaneousVelocity.x = Mathf.MoveTowards(instantaneousVelocity.x,
                                                        thumbstick.x * runTopSpeed,
                                                        runAcceleration * Time.fixedDeltaTime);
        }
    }


    [Header("Gravity Properties")]
    [SerializeField] public float fallTopSpeed;
    [SerializeField] public float fallAcceleration;

    private void PlayerGravity()
    {
        if (grounded && instantaneousVelocity.y < 0) { instantaneousVelocity.y = 0; }
        else
        {
            instantaneousVelocity.y = Mathf.MoveTowards(instantaneousVelocity.y,
                                                        -fallTopSpeed,
                                                        fallAcceleration * Time.fixedDeltaTime);
        }

    }

}
