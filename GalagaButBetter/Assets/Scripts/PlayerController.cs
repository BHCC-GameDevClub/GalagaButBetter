using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
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

    // Dash Variabls & Event
    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.02f;
    public float dashCooldown = 2f;

    private bool isDashing = false;
    private bool canDash = true;

    public static event Action<bool> OnDashStateChanged;

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

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        OnDashStateChanged?.Invoke(true); // broadcast to UI at start
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }
        
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
        if (isDashing) return; // Locked in dash
        Vector3 movement = new Vector3(move.x, move.y, 0f); // Not moving on the Z axis leave 0f
        transform.Translate(movement * speed * Time.deltaTime, Space.World); // Allows character to move to directed point
    }

    public void movePlayerWithAim()
    {
        if (isDashing) return; // Locked in dash
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

    private IEnumerator DashCoroutine()
    {   // Dash Start
        canDash = false;
        isDashing = true;
        OnDashStateChanged?.Invoke(false); // broadcast to UI dash on Cooldown

        Vector3 dashDirection;
        if (move.sqrMagnitude > 0.01f)
        {
            dashDirection = new Vector3(move.x, move.y, 0f).normalized; // direction dash
        }
        else
        {
            dashDirection = transform.up; // idle dash where facing
        }

        float dashStartTime = Time.time;
        while (Time.time < dashStartTime + dashDuration)
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);
            yield return null;
        }

        // Dash End & Cooldowns
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
        OnDashStateChanged?.Invoke(true); // Broadcast to UI dash ready
    }
        
}
