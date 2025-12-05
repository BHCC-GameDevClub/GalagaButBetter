using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Camera myCamera;
    
    [Header("Locked Y Positions")]
    public float plane1Y = 5f;  
    public float plane2Y = 36.8f; 

    [Header("Scroll Settings")]
    public float minScrollSpeed = 6f; // speed at what camera moves
    public float lookAheadX = 2.0f; // Look ahead

    [Header("Teleport Smoothness")]
    public float jumpDuration = 0.25f; // Camera slied time

    [Header("Proximity Acceleration")]
    public float boostCenterOffset = 2.0f; // distance from the center where to boost
    public float boostMaxDistance = 4.0f; // distance from center max speed
    public float maxBoostSpeed = 20.0f; // max speed camera can reach
    private float currentBoostSpeed = 0f; // stores final calculated speed

    [HideInInspector] public bool isTeleporting = false;
    private float currentLockedY; // Y Lock
    private const float LockedZ = -25f; // Camera depth

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollow target is not assigned!");
            enabled = false;
            return;
        }

        currentLockedY = plane1Y; 
        transform.position = new Vector3(target.position.x, currentLockedY, LockedZ);

        myCamera = GetComponent<Camera>();
        if (myCamera == null)
        {
            if (Camera.main != null)
            {
                myCamera = Camera.main;
            }
            else
            {
            Debug.LogError("CameraFollow must be attached");
            enabled = false;
            return;
            }
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (isTeleporting)
        {
            transform.position = new Vector3(
                transform.position.x,
                currentLockedY,
                LockedZ
            );
            return;
        }

        // ======================== [Acceleration Logic]

        // Calculator
        Vector3 cameraPosition = transform.position;
        float distanceFromCenter = target.position.x - cameraPosition.x; // use player current position

        // currentBoostSpeed to min pace
        currentBoostSpeed = minScrollSpeed;

        // Zone Limits Defined
        float zoneStart = boostCenterOffset;
        float zoneEnd = boostMaxDistance;
        float zoneRange = zoneEnd - zoneStart;

        if (distanceFromCenter > zoneStart)
        {
            // calc progress
            float progress = Mathf.Clamp01((distanceFromCenter - zoneStart) / zoneRange);

            // Interpolate speed
            currentBoostSpeed = Mathf.Lerp(minScrollSpeed, maxBoostSpeed, progress);
        }
        
        
        // ======================== [Trailing Movement Logic]

        // Calculate target X
        float targetX = cameraPosition.x + currentBoostSpeed * Time.deltaTime;

        // check player pull
        float playerx = target.position.x;
        float deltaX = playerx - targetX;

        if (deltaX > lookAheadX)
        {
            targetX = playerx - lookAheadX;
        }

        // anti reversal clamp
        targetX = Mathf.Max(targetX, cameraPosition.x);
        


        // ======================== [Apply FInal Position]
        Vector3 newPosition = new Vector3(
            targetX,
            currentLockedY,
            LockedZ
        );
        
        transform.position = newPosition;
    }    

    public void TeleportCameraToPlane(int planeIndex, float newX)
    {
        Debug.Log("Camera received command" + planeIndex);
        // Set the new locked Y-coordinate instantly
        if (planeIndex == 1)
        {
            currentLockedY = plane1Y;
        }
        else if (planeIndex == 2)
        {
            currentLockedY = plane2Y;
        }
        
        // camera slide
        StartCoroutine(SmoothJumpY(newX));
    }

    IEnumerator SmoothJumpY(float targetX) // camera's desired static
    {
        float startY = transform.position.y;
        float elapsedTime =  0f;
        float currentCameraX = transform.position.x; // Store X axis

        while (elapsedTime < jumpDuration)
        {
            // Smooth over time calc
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / jumpDuration);

            // smooth Y interpolation
            float newY = Mathf.Lerp(startY, currentLockedY, progress);

            // X position = Target X
            transform.position = new Vector3(
                currentCameraX,
                newY,
                LockedZ
          );
          yield return null;

        }
        transform.position = new Vector3(currentCameraX, currentLockedY, LockedZ); // precision Y landing
    }
}