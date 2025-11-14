using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ScrollingBackground : MonoBehaviour
{
    [Tooltip("How fast the background moves to the left.")]
    public float scrollSpeed;

    [Tooltip("The width of this background image in Unity units.")]
    public float backgroundWidth;
    private Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Starting position stored
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newX = Mathf.Repeat(Time.time * scrollSpeed, backgroundWidth);

        transform.position = startPosition + Vector3.left * newX;
    }
}
