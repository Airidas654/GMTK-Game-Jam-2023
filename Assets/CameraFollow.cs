using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform target; // Reference to the player object
    [SerializeField] Vector3 offset; // Offset of the camera from the player

    [SerializeField] float smoothSpeed = 0.3f;
    Vector2 vel;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 newpos = Vector2.SmoothDamp(transform.position, target.position + offset, ref vel, smoothSpeed);
            transform.position = new Vector3(newpos.x, newpos.y, transform.position.z);
        }
    }
}
