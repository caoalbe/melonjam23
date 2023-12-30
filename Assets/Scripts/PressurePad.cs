using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePad : MonoBehaviour
{
    private int totalWeight;
    public UnityEvent activatePadEvent; // somebody walks onto
    public UnityEvent deactivatePadEvent; // nobody is ontop

    // Start is called before the first frame update
    void Start()
    {
        totalWeight = 0;
    }

    // Walking onto pad
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: support varying weight?
        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            if (totalWeight == 0) { activatePadEvent.Invoke(); }
            totalWeight++;
        }
    }

    // Walking off of pad
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            if (totalWeight == 1) { deactivatePadEvent.Invoke(); }
            totalWeight--;
        }
    }
}
