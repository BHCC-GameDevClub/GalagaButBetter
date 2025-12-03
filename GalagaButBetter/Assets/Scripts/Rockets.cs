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
        Vector3 launchDirection = new Vector3(direction.x, direction.y, 0).normalized;
        rb.linearVelocity = launchDirection * speed;
        // Keeps player facing right
       transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        // Keep rockets on correct plane
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
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
