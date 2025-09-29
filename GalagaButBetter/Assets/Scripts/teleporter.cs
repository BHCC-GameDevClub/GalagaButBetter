using Unity.VisualScripting;
using UnityEngine;

public class Teleporter : MonoBehaviour

{
    public Transform Player;
    private float plane1 = 0f; // Plane 1 cords
    private float plane2 = 35f; // Plane 2 cords
    private bool atPlane = true; // Bool to track current location

    void Update()
    {
        if (Input.GetButtonDown("Jump")) // Jump aka teleport Input
        {
            TeleportPlayer();
        }
    }

    public void TeleportPlayer()
    {
        if (Player != null)
        {
            Vector3 currentPosition = Player.position;
            Vector3 newPosition;
            if (atPlane) // position check
            {
                newPosition = new Vector3(currentPosition.x, plane2, currentPosition.z); // teleport to plane2 if on plane1
            }
            else
            {
                newPosition = new Vector3(currentPosition.x, plane1, currentPosition.z); // teleport to plane1 if on plane2
            }
            Player.position = newPosition;
            atPlane = !atPlane; // position toggle
        }
    }
}