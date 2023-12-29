using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:
// Coyote Time
// Jump Buffering

public class PlatformerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public CapsuleCollider2D coll; // assuming its a rectangular sprite

    private Vector2 thumbstick;
    private bool pressedJump;
    private Vector2 instantaneousVelocity;
    private float currTime = 0f;

    void Start()
    {
        transform.position = Backend.instance.GetCheckPoint();
    }

    void Update()
    {
        // fetch controls
        if (controlLoss)
        {
            thumbstick.x = 0f;
            thumbstick.y = 0f;
            pressedJump = false;
        }
        else
        {
            thumbstick.x = Input.GetAxisRaw("Horizontal");
            thumbstick.y = Input.GetAxisRaw("Vertical");
            pressedJump = Input.GetButton("Jump");
        }

        // face character in correct direction
        if (thumbstick.x < 0)
        {
            // face left
            transform.localScale = leftScale;
        }
        else if (thumbstick.x > 0)
        {
            // face right
            transform.localScale = rightScale;
        }

        // regain control after knockback
        if (lastHitTime + controlLossDuration < currTime)
        {
            controlLoss = false;
        }

        currTime += Time.deltaTime;
    }

    void FixedUpdate()
    {
        CheckCollisions();

        // Compute the instantaneous velocity
        PlayerJump();
        HorizontalVelocity();
        PlayerKnockback();
        PlayerGravity();

        rb.velocity = instantaneousVelocity; // Apply the computed velocity
    }

    [Header("Collision Properties")]
    [SerializeField] public float detectionDistance;
    [SerializeField] public Vector2 raycastSize;

    private void CheckCollisions()
    {
        bool stashStartInColliders = Physics2D.queriesStartInColliders;
        Physics2D.queriesStartInColliders = false;

        // Raycast up and down
        bool hitGround = Physics2D.CapsuleCast(coll.bounds.center,
                                            raycastSize,
                                            coll.direction,
                                            0,
                                            Vector2.down,
                                            detectionDistance,
                                            LayerMask.GetMask("Terrain"));

        bool hitCeiling = Physics2D.CapsuleCast(coll.bounds.center,
                                            raycastSize,
                                            coll.direction,
                                            0,
                                            Vector2.up,
                                            detectionDistance,
                                            LayerMask.GetMask("Terrain"));

        // Hit a ceiling
        if (hitCeiling) { instantaneousVelocity.y = Mathf.Min(0, instantaneousVelocity.y); }

        // Landed on Ground
        if (hitGround && !grounded) { grounded = true; }

        // Leaving the Ground
        if (!hitGround && grounded) { grounded = false; }

        Physics2D.queriesStartInColliders = stashStartInColliders;
    }

    [Header("Jumping Properties")]
    [SerializeField] public float jumpHeight;
    private bool grounded;

    private void PlayerJump()
    {
        if (grounded && pressedJump)
        {
            instantaneousVelocity.y = Mathf.Sqrt(2 * fallAcceleration * (jumpHeight + 0.25f));
        }
    }


    [Header("Running Properties")]
    [SerializeField] public float runTopSpeed;
    [SerializeField] public float runAcceleration;
    [SerializeField] public float runDeceleration;

    private void HorizontalVelocity()
    {
        if (controlLoss) { return; }

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

    [Header("Knockback Properties")]
    [SerializeField] public float enemyKnockBackSpeed;
    [SerializeField] public float angleKnockBack; // in degrees
    [SerializeField] public float controlLossDuration;
    private bool controlLoss = false;
    private float lastHitTime;
    private Vector2 kbDir = new Vector2();

    private Vector2 sporadicVelocity = Vector2.zero;
    private void PlayerKnockback()
    {
        if (sporadicVelocity != Vector2.zero)
        {
            instantaneousVelocity.x = sporadicVelocity.x;
            instantaneousVelocity.y = sporadicVelocity.y;
            sporadicVelocity = Vector2.zero;
        }

    }

    [Header("Misc.")]
    [SerializeField] private Vector3 leftScale;
    [SerializeField] private Vector3 rightScale;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            float angleRad = angleKnockBack / 180 * Mathf.PI;
            kbDir.x = Mathf.Cos(angleRad);
            kbDir.y = Mathf.Sin(angleRad);
            if (transform.position.x - collision.gameObject.transform.position.x < 0)
            {
                kbDir.x *= -1;
            }

            sporadicVelocity += kbDir * enemyKnockBackSpeed;
            lastHitTime = currTime;
            controlLoss = true;
        }
    }
}
