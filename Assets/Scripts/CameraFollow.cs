using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform target; // Reference to the player object

    Vector2 CameraBorders;
    [SerializeField] float WeightToTarget; // 0 camera in position (0,0), 1 camera on player

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        Camera cam = GetComponent<Camera>();
        Vector2 camerasize = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 borders = GameManager.Instance.WorldBorders;
        CameraBorders = new Vector2(Mathf.Max(0, borders.x - camerasize.x), Mathf.Max(0, borders.y - camerasize.y));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(Mathf.Clamp(target.position.x * WeightToTarget, -CameraBorders.x, CameraBorders.x), Mathf.Clamp(target.position.y * WeightToTarget, -CameraBorders.y, CameraBorders.y), transform.position.z);
        }
    }
}
