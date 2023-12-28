using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private GameObject targetObj;
    [SerializeField] private Vector2 followOffset;
    [SerializeField] private float cameraSpeed;
    private Vector2 threshhold;
    private Rigidbody2D rb;

    void Start()
    {
        transform.position = new Vector3(targetObj.transform.position.x,
                                            targetObj.transform.position.y,
                                            transform.position.z);
        threshhold = CalculateThreshold();
        rb = targetObj.GetComponent<Rigidbody2D>();
    }

    float xDiff;
    float yDiff;
    Vector3 newPosition = Vector3.zero;
    void LateUpdate()
    {
        Vector2 target = targetObj.transform.position;
        xDiff = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * target.x);
        yDiff = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * target.y);

        newPosition = transform.position;
        if (Mathf.Abs(xDiff) >= threshhold.x) { newPosition.x = target.x; }
        if (Mathf.Abs(yDiff) >= threshhold.y) { newPosition.y = target.y; }

        transform.position = Vector3.MoveTowards(transform.position,
                                                newPosition,
                                                Mathf.Max(cameraSpeed, rb.velocity.magnitude) * Time.deltaTime);
    }



    private Vector3 CalculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 output = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height,
                                        Camera.main.orthographicSize);
        return output - followOffset;
    }

    // Draw the camera bounding box
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue;
    //     Vector2 border = CalculateThreshold();
    //     Gizmos.DrawWireCube(transform.position, new Vector3(2 * border.x, 2 * border.y, 1));
    // }


}
