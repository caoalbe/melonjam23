using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    public float speed = 1.0f;
    private bool isPlayerInvincible = false;
    public Collider2D enemyCollider;
    public Rigidbody2D rb;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player && !isPlayerInvincible)
        {
            var step = speed * Time.deltaTime;
            Vector2 goalPos = new Vector2(player.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, goalPos, step);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(CollideWithPlayer(collision.gameObject.transform.position));
        }
    }

    private IEnumerator CollideWithPlayer(Vector2 playerPos)
    {
        isPlayerInvincible = true;
        Backend.instance.TakeDamage(1);

        yield return new WaitForSeconds(1);

        isPlayerInvincible = false;
    }
}
