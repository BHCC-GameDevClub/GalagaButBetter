using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Enemy_Health))]
public class TwoPhaseEnemy : MonoBehaviour
{
    [Header("Phase 1: Scroll & Shoot")]
    public float scrollOffsetX = 30f; // distance from left edge
    public float shootingInterval = 2f; // dps
    public float entrySpeed = 3f;
    public GameObject projectilePrefab;

    [Header("Phase 2: Charge")]
    public float chargeSpeed = 8f;
    public float loopOffset = 50f;

    // Internal states
    private Animator animator;
    private Camera mainCam;
    private Enemy_Health healthScript;
    private bool isCharging = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCam = Camera.main;
        healthScript = GetComponent<Enemy_Health>();

        // Listen for DMG
        if (healthScript != null)
        {
            healthScript.OnHealthChanged += CheckDamageThreshold;
        }

        // Phase 1 Ranged
        InvokeRepeating(nameof(Shoot), shootingInterval, shootingInterval);
    }

    void OnDestroy()
    {
        if (healthScript != null)
        {
            healthScript.OnHealthChanged -= CheckDamageThreshold;
        }
    }

    void CheckDamageThreshold(int current, int max)
    {
        // if 50% >= less health and is not already charging
        if (!isCharging && current <= (max * 0.5f))
        {
            StartCharge();
        }
    }

    void Update()
    {
        if (isCharging)
        {
            // Phase 2 Logic
            // 1. Move Left
            transform.Translate(Vector3.left * (chargeSpeed * Time.deltaTime));

            // 2. Loop Charge
            if (mainCam != null)
            {
                float leftEdge = mainCam.transform.position.x - loopOffset; // Screen Edge

                if (transform.position.x < leftEdge)
                {
                    // Reset Position
                    Vector3 respawnPos = transform.position;
                    respawnPos.x = mainCam.transform.position.x + loopOffset;
                    transform.position = respawnPos;
                }
            }
        }
        else
        {
            // Phase 1 Logic
            // Offset
            if (mainCam != null)
            {
                float targetX = mainCam.transform.position.x + scrollOffsetX;

                if (transform.position.x > targetX)
                {
                    // Smooth Entry
                    Vector3 newPos = transform.position;
                    newPos.x = Mathf.MoveTowards(newPos.x, targetX, entrySpeed * Time.deltaTime);
                    transform.position = newPos;
                }
                else
                {
                    // Locked
                    Vector3 lockedPos = transform.position;
                    lockedPos.x = targetX;
                    transform.position = lockedPos;
                }
            }
        }
    }

    void Shoot()
    {
        // Debugging
        // Debug.Log("Shoot() called. Charging? " + isCharging);

        // Find Player
        PlayerHealth player = FindFirstObjectByType<PlayerHealth>();

        if (projectilePrefab != null && !isCharging)
        {
            Quaternion spawnRotation;

            if (player != null)
            {
                // Target Start
                Vector3 direction = player.transform.position - transform.position;

                // rotation
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // spawn rotation
                spawnRotation = Quaternion.Euler(0f, 0f, angle);
            }
            else
            {
                // Default
                spawnRotation = Quaternion.Euler(0f, 0f, 180f);
            }

            Instantiate(projectilePrefab, transform.position, spawnRotation);
        }
    }

    void StartCharge()
    {
        isCharging = true;
        CancelInvoke(nameof(Shoot)); // stop shooting
        if (animator != null) animator.SetTrigger("Charge");
    }

}
