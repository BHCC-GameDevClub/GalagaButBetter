using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rockets : MonoBehaviour
{

    // public Vector3 direction = Vector3.forward; //Vector3.right
    public float lifespawn = 2f;
    public float speed;
    private Rigidbody rb; // keep private
    public int damage = 1; // rocket damage

    void Awake()
    {
        // Get the Rigidbody component on same object
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction)
    {
        // Leave Y 0 to ensure rocket only goes X-Z planes
        Vector3 flatDirection = new Vector3(direction.x, 0, direction.z).normalized;
        rb.linearVelocity = flatDirection * speed;
        // Rotates the rocket to new movement direction
        if (flatDirection != Vector3.zero)
        {
            // Face direction of travel
            Quaternion lookRotation = Quaternion.LookRotation(flatDirection);
            // 90-degree rotation on X for asset flip
            Quaternion xRotation = Quaternion.Euler(90, 0, 90);
            // Combine the two
            transform.rotation = lookRotation * xRotation;
        }
        // Keep rockets from going up or down Y
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        Destroy(gameObject, lifespawn);
    }

    private void OnCollisionEnter(Collision collision) // collision needed for damage output
    {
        Enemy_Health enemyHealth = collision.gameObject.GetComponent<Enemy_Health>();
        if (enemyHealth != null)
        {
            enemyHealth.ChangeHealth(damage);
        }
        Destroy(gameObject);
    }
}
