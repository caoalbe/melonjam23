using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private CapsuleCollider2D enemyCollider;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public float viewAngle;
    [SerializeField] LayerMask obstacleMask;
    private GameObject player;
    private bool grounded = false;
    private Vector2 instantaneousVelocity = new Vector2(0, 0);

    void FixedUpdate()
    {
        // Print(rb.velocity.x);
        if (player)
        {
            // Check Grounded
            bool stashStartInColliders = Physics2D.queriesStartInColliders;
            Physics2D.queriesStartInColliders = false;

            bool hitGround = Physics2D.CapsuleCast(enemyCollider.bounds.center,
                                            new Vector2(0.85f, 1.0f),
                                            enemyCollider.direction,
                                            0,
                                            Vector2.down,
                                            0.05f,
                                            LayerMask.GetMask("Terrain"));

            if (hitGround && !grounded) { grounded = true; } // Landed on Ground
            if (!hitGround && grounded) { grounded = false; } // Leaving the Ground

            Physics2D.queriesStartInColliders = stashStartInColliders;

            // Move left-right
            instantaneousVelocity.x = speed * Time.fixedDeltaTime;
            if (player.transform.position.x < transform.position.x)
            {
                instantaneousVelocity.x *= -1;
            }

            // Apply Gravity
            if (grounded && instantaneousVelocity.y < 0) { instantaneousVelocity.y = 0; }
            else
            {
                instantaneousVelocity.y = Mathf.MoveTowards(instantaneousVelocity.y,
                                                            -50.0f,
                                                            85.0f * Time.fixedDeltaTime);
            }
        }
        else
        {
            // TODO: implement idle behaviour
        }
        rb.velocity = instantaneousVelocity;
    }

    // Player Detection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Vector3 dirToTarget = (collision.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, collision.transform.position);
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    player = collision.gameObject;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = null;
        }
    }

    // Deal Damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerInfo>().TakeDamage(1);
        }
    }
}
