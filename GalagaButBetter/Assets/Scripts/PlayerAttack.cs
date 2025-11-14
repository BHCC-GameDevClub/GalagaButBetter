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
    public Transform LaunchPoint2;
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

        if (LaunchPoint2 != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(LaunchPoint2.position, weaponRange);
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
        Debug.Log("Shoot method called!"); // debug log for shooting
        Vector3 launchDirection = transform.up;
      
        Rockets rocketL = Instantiate(RocketPrefab, LaunchPoint.position, Quaternion.identity).GetComponent<Rockets>();
        Rockets rocketR = Instantiate(RocketPrefab, LaunchPoint2.position, Quaternion.identity).GetComponent<Rockets>();

        // launch method for both rockets
        rocketL.Launch(launchDirection);
        rocketR.Launch(launchDirection);

        // CloseRange Component
        List<Collider> detectedEnemies = new List<Collider>(); //collider list
        detectedEnemies.AddRange(Physics.OverlapSphere(LaunchPoint.position, weaponRange, enemyLayer));
        detectedEnemies.AddRange(Physics.OverlapSphere(LaunchPoint2.position, weaponRange, enemyLayer));
        Collider[] enemies = detectedEnemies.ToArray(); // List into array just incase
        foreach (Collider enemyCollider in detectedEnemies)
        {
            Enemy_Health enemyHealth = enemyCollider.GetComponent<Enemy_Health>();
            if (enemyHealth != null)
            {
                enemyHealth.ChangeHealth(damage);
            }
        }

            // Collider[] enemies = Physics.OverlapSphere(LaunchPoint.position, weaponRange, enemyLayer);
            // Collider[] enemies2 = Physics.OverlapSphere(LaunchPoint2.position, weaponRange, enemyLayer);


            shootTimer = shootCooldown;
    }
}
