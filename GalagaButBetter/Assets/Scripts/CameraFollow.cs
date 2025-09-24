using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public Transform target; // Camera follows
    public float smoothTime = 0.3f; // time takes for camera to follow
    public Vector3 offset; // Offset between camera and holder
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update

void Start()
    {
    }

// Update is called once per frame
void Update()
    {
     if (target != null) // confirms target and shouldn't mess up camera on death
    {
        Vector3 targetPosition = target.position + offset; // Point of character position + offset
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
