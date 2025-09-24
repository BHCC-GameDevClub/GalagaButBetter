using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform LaunchPoint;
    public Transform LaunchPoint2;
    public GameObject LeftRocketPrefab;
    public GameObject RightRocketPrefab;
    private Vector2 aimDirection = Vector2.right;
    public float shootCooldown = .5f;
    private float shootTimer;
    public Camera MainCamera;


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

        shootTimer = shootCooldown;
    }
}
