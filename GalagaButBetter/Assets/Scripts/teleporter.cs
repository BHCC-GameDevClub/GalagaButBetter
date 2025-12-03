using Unity.VisualScripting;
using UnityEngine;

public class Teleporter : MonoBehaviour

{
    public Transform Player;
    public CameraFollow cameraFollowScript;
    private const float Plane1Y = 5f;
    private const float Plane2Y = 35f;
    private float plane1 = 30f; // Plane 1 cords
    private float plane2 = 30f; // Plane 2 cords
    private bool atPlane = true; // Bool to track current location

    void Start()
    {
        if (cameraFollowScript == null)
        {
            cameraFollowScript = Camera.main.GetComponent<CameraFollow>();
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump")) // Jump aka teleport Input
        {
            PerformTeleportSequence();
        }
    }

    public void PerformTeleportSequence()
    {
        TeleportPlayer();
        TeleportCamera();
        atPlane = !atPlane;
    }

    public void TeleportPlayer()
    {
        if (Player != null)
        {
            Vector3 currentPosition = Player.position;
            Vector3 newPosition;
            if (atPlane) // position check
            {
                newPosition = new Vector3(currentPosition.x, plane2 + currentPosition.y, currentPosition.z); // teleport to plane2 if on plane1
            }
            else
            {
                newPosition = new Vector3(currentPosition.x, currentPosition.y - plane1, currentPosition.z); // teleport to plane1 if on plane2
            }
            Rigidbody rb = Player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.position = newPosition;
            }
            else
            {
                Player.position = newPosition;
            }
        }
    }
    public void TeleportCamera()
    {
        if (cameraFollowScript == null) return;
        int targetPlaneIndex = atPlane ? 2 : 1;
        cameraFollowScript.TeleportCameraToPlane(targetPlaneIndex);
    }
}