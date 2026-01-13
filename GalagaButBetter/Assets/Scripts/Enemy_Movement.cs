using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    private Rigidbody rb;
    private Transform currentPos;
    public float speed = 3f;
    private float leftbound = -60;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= leftbound)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(-speed, rb.linearVelocity.y, rb.linearVelocity.z);
        //or can use transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

}
