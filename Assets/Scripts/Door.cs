using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector3 closedPos;
    [SerializeField] private Vector3 openPos;
    [SerializeField] private float doorSpeed;
    [SerializeField] private float frameRate;

    private Coroutine activeCoroutine;
    private float framePeriod;

    void Start() {
        framePeriod = 1.0f/frameRate;
    }

    public void OpenDoor()
    {
        if (activeCoroutine != null) { StopCoroutine(activeCoroutine); }

        activeCoroutine = StartCoroutine(OpenAnim());
    }

    public void CloseDoor()
    {
        if (activeCoroutine != null) { StopCoroutine(activeCoroutine); }

        activeCoroutine = StartCoroutine(CloseAnim());
    }

    private IEnumerator OpenAnim()
    {
        while (transform.position != openPos) 
        {
            transform.position = Vector3.MoveTowards(transform.position, openPos, doorSpeed * framePeriod);
            yield return new WaitForSeconds(framePeriod);
        }
    }

    private IEnumerator CloseAnim()
    {
        while (transform.position != closedPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, closedPos, doorSpeed * framePeriod);
            yield return new WaitForSeconds(framePeriod);
        }
    }
}
