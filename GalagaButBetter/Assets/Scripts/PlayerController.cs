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
    private Rigidbody rb;
    [Header("Constant Movement")]
    public float baseForwardSpeed = 2.5f; // idle speed
    public float speed = 5f; // speed of character
    private Vector2 move; // store input values
    public bool isPc; // Checks for Gamepad or M&K

    // Dash Variabls & Event
    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.02f;
    public float dashCooldown = 2f;
    [Header("Collision Settings")]
    public LayerMask dashStopLayer;
    private bool isDashing = false;
    private bool canDash = true;

    // Camera
    [Header("Boundaries")]
    public Camera mainCamera; // Main camera component
    public float leftBoundaryBuffer = 1.0f; // Left screen limiter


    public static event Action<bool> OnDashStateChanged;

    // Movement Input Calls
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
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
        transform.rotation = Quaternion.identity; // Rotation 0
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Player needs rigidbody");
            enabled = false;
            return;

        }
        rb.constraints |= RigidbodyConstraints.FreezeRotation;
        
        OnDashStateChanged?.Invoke(true); // broadcast to UI at start
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PauseMenu.GameIsPaused)
        {
            rb.linearVelocity = UnityEngine.Vector3.zero;
            return;
        }
        if (isDashing)
        {
            return;
        }
        movePlayer();

    // Left Boundary
        if (mainCamera != null)
        {
            CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();
            if (playerCollider == null)
            {
                Debug.LogError("missing CapsuleCollider Component");
                return;
            }
            float playerZDepth = transform.position.z;
            Vector3 leftEdgeWorld = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, playerZDepth));
            float minPlayerX = leftEdgeWorld.x;
            float clampedX= Mathf.Max(transform.position.x, minPlayerX);

/*           float cameraHalfHeight = mainCamera.orthographicSize;
           float cameraHalfWidth = cameraHalfHeight * mainCamera.aspect;
           float leftCameraEdgeX = mainCamera.transform.position.x - cameraHalfWidth;
           float minPlayerX = leftCameraEdgeX;
           float clampedX = Mathf.Max(transform.position.x, minPlayerX); */
           transform.position = new Vector3(
            clampedX,
            transform.position.y,
            transform.position.z
           );
        } 
        // Debug for camera boundary
         //Debug.Log($"Cam X: {mainCamera.transform.position.x}, Half Width: {cameraHalfWidth}, Min X: {minPlayerX}");
    }

    public void movePlayer()  // WASD/LTS input for movement
    {   
        Vector3 inputMovement = new UnityEngine.Vector3(move.x, move.y, 0f) * speed;
        Vector3 forwardMovement = UnityEngine.Vector3.right * baseForwardSpeed; // constant movement to right
        Vector3 netDesiredVelocity = forwardMovement + inputMovement;
        rb.linearVelocity = netDesiredVelocity;
    }

    private IEnumerator DashCoroutine()
    {   // Dash Start
        canDash = false;
        isDashing = true;
        OnDashStateChanged?.Invoke(false); // dash broadcast to UI Cooldown

        Vector3 dashDirection;
        if (move.sqrMagnitude > 0.01f)
        {
            dashDirection = new UnityEngine.Vector3(move.x, move.y, 0f).normalized; // direction dash
        }
        else
        {
            dashDirection = transform.right; // idle dash where facing
        }
        
        rb.linearVelocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        
        // Dash End & Cooldowns
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
        OnDashStateChanged?.Invoke(true); // Broadcast to UI dash ready
    }

    public Vector2 GetMoveInput()
    {
        return move;
    }
        
}
