using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveCircle : MonoBehaviour
{
    [SerializeField]
    float speed = 10f;
    public Vector3 targetPosition;

    // Update is called once per frame
    void Update()
    {
        // The step means by how much should the object move in a frame.
        // The deltaTime represents the interval in seconds between the last frame and the current one.
        float step = speed * Time.deltaTime;
        // This line moves our object to the end position.
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }
}
