using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    [SerializeField] private CubeMover cubeMover;
    [SerializeField] private float speed;
    private Vector3 lastPos;
    

    private void Update()
    {
        HandleRotation();
    }

    private Vector3? deltaRotation
    {
        get
        {
            if (Input.GetMouseButtonDown(1))
            {
                lastPos = Input.mousePosition;
                return null;
            }
            else if (Input.GetMouseButton(1))
            {
                Vector2 delta = Input.mousePosition - lastPos;
                lastPos = Input.mousePosition;
                return new Vector3(delta.y, -delta.x, 0) * speed;
            }
            return null;
        }
    }

    private void HandleRotation()
    {
        if (cubeMover.isMoving)
        {
            lastPos = Input.mousePosition;
            return;
        }

        var rotation = deltaRotation;

        if (!rotation.HasValue) return;

        this.transform.RotateAround(this.transform.position, Vector3.right, rotation.Value.x);
        this.transform.RotateAround(this.transform.position, Vector3.up, rotation.Value.y);
    }
}
