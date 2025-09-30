using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;


    private void OnCollisionEnter(Collision collision) //collision will refer to player when he collides w enemy
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ChangeHealth(-damage);
        }
        //collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
    }
}
