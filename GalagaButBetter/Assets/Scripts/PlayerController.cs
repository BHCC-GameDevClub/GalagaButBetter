using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
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
     
    }

// Update is called once per frame
void Update()
    {
        if (isPc) // Checks for Gamepad controller if and else
        {
            Vector3 mouseScreenPos = mouseLook; // mouse is now world position
            mouseScreenPos.z = Camera.main.nearClipPlane + 10f; // Z value is distance from camera to point
            rotationTarget = Camera.main.ScreenToWorldPoint(mouseScreenPos);

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
        Vector3 movement = new Vector3(move.x, move.y, 0f); // Not moving on the Z axis leave 0f
        transform.Translate(movement * speed * Time.deltaTime, Space.World); // Allows character to move to directed point
    }

public void movePlayerWithAim()
    {

        if (isPc) //Check for PC or controller
        {
            Vector3 aimDirection = (rotationTarget - transform.position);
            if (aimDirection.sqrMagnitude > 0.01f) // tiny deadzone on mouse so it doesnt snap
            {
                float angle = (Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg) - 90f;
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
                transform.rotation = targetRotation;
            }
        }
        else
        {
            Vector3 aimDirection = new Vector3(joystickLook.x, joystickLook.y, 0f);

            if (aimDirection.sqrMagnitude > 0.01f)
            {
                float angle = (Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg) - 90f;
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);
            }
        }

        Vector3 movement = new Vector3(move.x, move.y, 0f);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

}
