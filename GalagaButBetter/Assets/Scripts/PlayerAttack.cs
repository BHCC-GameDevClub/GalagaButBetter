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
    public GameObject LeftRocketPrefab;
    public GameObject RightRocketPrefab;
    private Vector2 aimDirection = Vector2.right;
    public float shootCooldown = .5f;
    private float shootTimer;
    public Camera MainCamera;

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
        HandleAiming();

        if (Input.GetButtonDown("Shoot") && shootTimer <= 0)
        {
            Shoot();
        }
    }

    private void HandleAiming()
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
        }
        }

    public void Shoot()
    {
        Debug.Log("Shoot method called!"); // debug log for shooting
        if (aimDirection == Vector2.zero)
        {
            // Mouseover player will default to direction
            aimDirection = new Vector2(transform.forward.x, transform.forward.z);
        }
        Rockets rocketL = Instantiate(LeftRocketPrefab, LaunchPoint.position, Quaternion.identity).GetComponent<Rockets>();
        Rockets rocketR = Instantiate(RightRocketPrefab, LaunchPoint2.position, Quaternion.identity).GetComponent<Rockets>();

        // launch method for both rockets
        rocketL.Launch(new Vector3(aimDirection.x, 0, aimDirection.y));
        rocketR.Launch(new Vector3(aimDirection.x, 0, aimDirection.y));

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
