using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Teleporter : MonoBehaviour

{

    public Transform Player;
    public CameraFollow cameraFollowScript;
    private bool atPlane = true; // Bool to track current location

    void Start()
    {
        // Safe initialization
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

    // Main sequence caller
    public void PerformTeleportSequence()
    {
        TeleportPlayer();
        atPlane = !atPlane;
    }

    // Player Teleport Logic
    public void TeleportPlayer()
    {
        // Abs coords from CameraFollow
        const float AbsolutePlane1Y = 5f;
        const float AbsolutePlane2Y = 36.8f;

        // const jump distance which should always be 31.8f
        const float VerticalJumpOffset = AbsolutePlane2Y - AbsolutePlane1Y;

        Vector3 currentPosition = Player.position;
        Vector3 newPosition;
        int targetPlaneIndex;

        Rigidbody rb = Player.GetComponent<Rigidbody>();

        // Debug Checks
        if (Player == null)
        {
            Debug.LogError("Teleporter player transform missing");
            return;
        }

        if (cameraFollowScript == null)
        {
            Debug.LogError("Teleporter CameraFollow script missing");
            return;
        }

        // Teleport sequence
        // Determines new position
        if (atPlane)
        {
            newPosition = new Vector3(currentPosition.x, currentPosition.y + VerticalJumpOffset, currentPosition.z);
            targetPlaneIndex = 2;
        }
        else
        {
            newPosition = new Vector3(currentPosition.x, currentPosition.y - VerticalJumpOffset, currentPosition.z);
            targetPlaneIndex = 1;
        }

        // Camera disable for x override
        cameraFollowScript.isTeleporting = true;

        // Safe RigidBody Teleport

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.position = newPosition;
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            Player.position = newPosition;
        }

        // Camera Teleport w Smooth Transition
        float finalCameraX = cameraFollowScript.transform.position.x;
        cameraFollowScript.TeleportCameraToPlane(targetPlaneIndex, finalCameraX);
        
        // Camera routine follow
        StartCoroutine(CameraResumeRoutine(0.01f));
    }

    // Coroutine Helper
    System.Collections.IEnumerator CameraResumeRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (cameraFollowScript != null)
        {
            cameraFollowScript.isTeleporting = false;
        }
    }
}