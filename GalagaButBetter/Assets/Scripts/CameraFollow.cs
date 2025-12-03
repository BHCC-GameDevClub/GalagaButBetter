using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    
    [Header("Locked Y Positions")]
    public float plane1Y = 5f;  
    public float plane2Y = 36.8f; 
    [Header("Scroll Settings")]
    public float minScrollSpeed = 4f; // speed at what camera moves
    public float lookAheadX = 2.0f; // Look ahead
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
    }

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 cameraPosition = transform.position;
        float targetX = cameraPosition.x; // move when player moves right
        targetX += minScrollSpeed * Time.deltaTime;
        float deltaX = target.position.x - targetX; // calculate diff

        // Move ahead threshold logic
        if (deltaX > lookAheadX)
        {
            targetX = target.position.x - lookAheadX;
        }

        // camera constraints
        Vector3 newPosition = new Vector3(
            targetX, 
            currentLockedY, 
            LockedZ
        );

        // Teleport logic
        transform.position = newPosition;
    }

    public void TeleportCameraToPlane(int planeIndex)
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
        
        // camera force jump
        transform.position = new Vector3(target.position.x, currentLockedY, LockedZ);
    }
}