using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CapsuleCollider2D enemyCollider;
    [SerializeField] private Rigidbody2D rb;

    [Header("Player Searching")]
    [SerializeField] public float viewAngle;
    [SerializeField] LayerMask obstacleMask;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float detectionDistance;
    [SerializeField] private Vector2 raycastSize;
    [SerializeField] private float gravityAcceleration;

    [Header("Misc.")]
    [SerializeField] private Vector3 leftScale;
    [SerializeField] private Vector3 rightScale;

    private GameObject player;
    private bool grounded = false;
    private Vector2 instantaneousVelocity = new Vector2(0, 0);

    [SerializeField] private Animator animator;

    void FixedUpdate()
    {
        // Check Grounded
        bool stashStartInColliders = Physics2D.queriesStartInColliders;
        Physics2D.queriesStartInColliders = false;

        bool hitGround = Physics2D.CapsuleCast(enemyCollider.bounds.center,
                                            raycastSize,
                                            enemyCollider.direction,
                                            0,
                                            Vector2.down,
                                            detectionDistance,
                                            LayerMask.GetMask("Terrain"));

        if (hitGround && !grounded) { grounded = true; } // Landed on Ground
        if (!hitGround && grounded) { grounded = false; } // Leaving the Ground

        Physics2D.queriesStartInColliders = stashStartInColliders;

        if (player)
        {
            // TODO: Use flip in sprite-renderer instead
            // Move left-right
            instantaneousVelocity.x = speed * Time.fixedDeltaTime;
            if (player.transform.position.x < transform.position.x)
            {
                instantaneousVelocity.x *= -1;
            }
        }
        else
        {
            // TODO: implement idle behaviour
        }

        // Apply Gravity
        if (grounded && instantaneousVelocity.y < 0) { instantaneousVelocity.y = 0; }
        else { instantaneousVelocity.y = -gravityAcceleration; }

        // face character in correct direction
        if (instantaneousVelocity.x < 0) { transform.localScale = leftScale; }
        else if (instantaneousVelocity.x > 0) { transform.localScale = rightScale; }

        rb.velocity = instantaneousVelocity;
        animator.SetFloat("VeloX", Mathf.Abs(rb.velocity.x));
    }

    // Player Detection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !collision.gameObject.GetComponent<PlayerInfo>().GetIsGhost())
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
            instantaneousVelocity.x = 0;
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
