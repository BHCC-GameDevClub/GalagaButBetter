using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float weaponRange = 2;
    public LayerMask enemyLayer;
    public int damage = 1;
    public Transform LaunchPoint;
    public GameObject RocketPrefab;
    public float shootCooldown = .5f;
    private float shootTimer;

    void OnDrawGizmosSelected()
    {
        if (LaunchPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(LaunchPoint.position, weaponRange);
        }
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Shoot") && shootTimer <= 0)
        {
            Shoot();
        }
    }


    //THIS IS CODE FOR AIMING WHEN TOP DOWN - DO NOT REMOVE

    /* private void HandleAiming()
    {
        // Mouse position on screen
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Ray from Camera going through position
        Ray ray = MainCamera.ScreenPointToRay(mouseScreenPosition);

        // Position Plane
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        float hitDistance = 0.0f;

        // Ray intersect check
        if (playerPlane.Raycast(ray, out hitDistance))
        {
            // Point where ray hits plane
            Vector3 targetPoint = ray.GetPoint(hitDistance);

            // direction from player to target point
            Vector3 direction = targetPoint - transform.position;
            aimDirection = new Vector2(direction.x, direction.z).normalized;
        } */


    public void Shoot()
    {
       Debug.Log("Shoot method call");
       
       Vector3 launchDirection = Vector3.right; // Fixed shooting position
       Rockets rocket = Instantiate(RocketPrefab, LaunchPoint.position, Quaternion.identity).GetComponent<Rockets>();

       rocket.Launch(launchDirection); // Launch Method
       
       List<Collider> detectedEnemies = new List<Collider>();
       detectedEnemies.AddRange(Physics.OverlapSphere(LaunchPoint.position, weaponRange, enemyLayer));

       Collider[] enemies = detectedEnemies.ToArray();
       foreach (Collider enemyCollider in detectedEnemies)
        {
            Enemy_Health enemyHealth = enemyCollider.GetComponent<Enemy_Health>();
            if (enemyHealth != null)
            {
                enemyHealth.ChangeHealth(damage);
            }
        }
        shootTimer = shootCooldown;
    }
}
