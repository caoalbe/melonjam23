using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private bool isPlayerDetected = false;
    private GameObject player;
    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            var step = speed * Time.deltaTime;
            Vector2 goalPos = new Vector2 (player.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, goalPos, step);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isPlayerDetected = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerDetected = false;
            player = null;
        }
    }
}
