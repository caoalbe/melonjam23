using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostForm : MonoBehaviour
{
    private bool isGhost = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetGhost(!isGhost);
        }
    }

    private void SetGhost(bool newForm)
    {
        isGhost = newForm;
        // TODO: implement this with changing sprites
        if (newForm)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
