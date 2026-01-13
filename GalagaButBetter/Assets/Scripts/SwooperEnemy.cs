using UnityEngine;

[RequireComponent(typeof(Enemy_Health))]
[RequireComponent(typeof(Enemy_Combat))]
public class SwooperEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeedX = 5f;
    public float moveSpeedY = 8f;
    public float resetBufferY = 22f; // respawn height

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null) mainCam = FindFirstObjectByType<Camera>();

        // Rotation Logic
        float angle = Mathf.Atan2(-moveSpeedY, -moveSpeedX) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }

    void Update()
    {
        // Down and Left
        Vector3 moveDir = (Vector3.down * moveSpeedY) + (Vector3.left * moveSpeedX);
        transform.Translate(moveDir * Time.deltaTime, Space.World);

        // Loop
        if (mainCam != null)
        {
            // Bottom of screen
            float bottomLimit = mainCam.transform.position.y - 20f;

            if (transform.position.y < bottomLimit)
            {
                RespawnAtTop();
            }
        }
    }

    void RespawnAtTop()
    {
        // Teleport
        Vector3 newPos = transform.position;
        newPos.y = mainCam.transform.position.y + resetBufferY;

        // Randomizer
        float randomXOffset = Random.Range(0f, 15f);
        newPos.x = mainCam.transform.position.x + randomXOffset;
        newPos.z = -7f;

        transform.position = newPos;
    }
}
