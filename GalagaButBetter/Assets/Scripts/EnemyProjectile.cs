using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public float lifetime = 5f; // Self-destruct timer

    void Start()
    {
        Destroy(gameObject, lifetime); // clean up
    }

    void Update()
    {
        //forward movement
        transform.Translate(Vector3.right * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        // ignore if hitting enemy
        if (other.CompareTag("Enemy")) return;

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.ChangeHealth(damage);
            Destroy(gameObject);

        }
    }



}
