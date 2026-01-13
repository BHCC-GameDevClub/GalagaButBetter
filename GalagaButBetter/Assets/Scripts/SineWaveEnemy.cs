using UnityEngine;

[RequireComponent(typeof(Enemy_Health))]
[RequireComponent(typeof(Enemy_Combat))]

public class SineWaveEnemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float waveFrequency = 2f;
    public float waveAmplitude = 5f;

    private Vector3 startPosition;
    private float timeCounter = 0f;
    private float leftbound = -60;

    public float rotationOffset = 180f;

    void Start()
    {
        startPosition = transform.position;
        timeCounter = Random.Range(0f, 2f * Mathf.PI); // Randomize starting point in wave
    }

    void Update()
    {
        timeCounter += Time.deltaTime * waveFrequency;

        // Calculate wave offset
        float waveOffset = Mathf.Sin(timeCounter) * waveAmplitude;

        // Move left with wave pattern
        Vector3 newPosition = new Vector3(
            startPosition.x - (moveSpeed * timeCounter / waveFrequency),
            startPosition.y + waveOffset,
            startPosition.z
        );

        Vector3 direction = newPosition - transform.position;

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
        }

        if (transform.position.x <= leftbound)
        {
            Destroy(gameObject);
        }

        transform.position = newPosition;
    }

    float CustomWaveFunction(float time)
    {
        // Example: Triangle wave pattern
        return Mathf.PingPong(time * 2f, 2f) - 1f;

        // Or: Sawtooth wave
        // return (time % 1f) * 2f - 1f;

        // Or: Square wave
        // return Mathf.Sin(time * 2f * Mathf.PI) > 0 ? 1f : -1f;
    }
}
