using Unity.VisualScripting;
using UnityEngine;

public class Teleporter : MonoBehaviour

{
    public Transform Player;
    private float plane1 = 0f;
    private float plane2 = 35f;
    private bool atPlane = true;

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
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
            if (atPlane)
            {
                newPosition = new Vector3(currentPosition.x, plane2, currentPosition.z);
            }
            else
            {
                newPosition = new Vector3(currentPosition.x, plane1, currentPosition.z);
            }
            Player.position = newPosition;
            atPlane = !atPlane;
        }
    }
}