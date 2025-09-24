using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Quaternion xAxisRotation;
    public float speed; // speed of character
    private Vector2 move, mouseLook, joystickLook; // store input values
    private Vector3 rotationTarget; // Point allowing look towards mouse position
    public bool isPc; // Checks for Gamepad or M&K

// Movement Input Calls
public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

public void OnJoystickLook(InputAction.CallbackContext context)
    {
        joystickLook = context.ReadValue<Vector2>();
    }

// Start is called before the first frame update
void Start()
    {
        // Keeps the sprite rotated 90 degrees on X
        xAxisRotation = Quaternion.Euler(90, 0, 0);
    }

// Update is called once per frame
void Update()
    {
        if (isPc) // Checks for Gamepad controller if and else
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouseLook);

            if (Physics.Raycast(ray, out hit))
            {
                rotationTarget = hit.point; // gives us vector3 to position
            }

            movePlayerWithAim();
        }
        else
        {
            if (joystickLook.x == 0 && joystickLook.y == 0) // Makes sure Joysticks stay seperate and only mvoement from left
            {
                movePlayer();
            }
            else
            {
                movePlayerWithAim();
            }
        }


    }

public void movePlayer()  // WASD/LTS action input for movement
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y); // Not moving on the Y axis leave 0f

        if (movement != Vector3.zero) // stops snapping to start position
        {
            // Gets horizontal rotation
            Quaternion horizontalRotation = Quaternion.LookRotation(movement);

            // Combines the rotations with frozen x rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, horizontalRotation * xAxisRotation, 0.15f); // 0.15f is speed
        }


        transform.Translate(movement * speed * Time.deltaTime, Space.World); // Allows character to move to directed point
    }

public void movePlayerWithAim()
    {
        if (isPc) //Check for PC or controller
        {
            var lookPos = rotationTarget - transform.position;
            lookPos.y = 0;
            var horizontalRotation = Quaternion.LookRotation(lookPos);

            Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

            if (aimDirection != Vector3.zero)
            {
                // Combines the rotation with frozen x rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, horizontalRotation * xAxisRotation, 0.15f);
            }
        }
        else
        {
            Vector3 aimDirection = new Vector3(joystickLook.x, 0f, joystickLook.y);

            if (aimDirection != Vector3.zero)
            {
                // gets horizontal rotation
                Quaternion horizontalRotation = Quaternion.LookRotation(aimDirection);

                // COmbines the rotation with frozen x
                transform.rotation = Quaternion.Slerp(transform.rotation, horizontalRotation * xAxisRotation, 0.15f);
            }
        }

        Vector3 movement = new Vector3(move.x, 0f, move.y);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

}
