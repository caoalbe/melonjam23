using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private GameObject trackingTarget;
    private Vector3 cameraPosition;

    [SerializeField] private float horizontalLerpValue;
    [SerializeField] private float verticalLerpValue;


    void Start()
    {
        // initialize camera position
        cameraPosition.x = trackingTarget.transform.position.x;
        cameraPosition.y = trackingTarget.transform.position.y;
        cameraPosition.z = -10;
    }

    void FixedUpdate()
    {
        cameraPosition.x = Mathf.Lerp(cameraPosition.x, trackingTarget.transform.position.x, horizontalLerpValue);
        cameraPosition.y = Mathf.Lerp(cameraPosition.y, trackingTarget.transform.position.y, verticalLerpValue);

        transform.position = cameraPosition;
    }
}
