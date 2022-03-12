using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    [SerializeField] private CubeMover cubeMover;
    [SerializeField] private float sensivity;
    private Vector3 lastPos;
    

    private void OnEnable()
    {
        EventSystem.OnRotationSensivityChanged += UpdateSensivity;
    }

    private void OnDisable()
    {
        EventSystem.OnRotationSensivityChanged -= UpdateSensivity;
    }

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
                return new Vector3(delta.y, -delta.x, 0) * sensivity;
            }
            return null;
        }
    }

    private bool canRotate => !cubeMover.isMoving && InputChecker.Instance.isEnabled;

    private void HandleRotation()
    {
        if (!canRotate)
        {
            lastPos = Input.mousePosition;
            return;
        }

        var rotation = deltaRotation;

        if (!rotation.HasValue) return;

        this.transform.RotateAround(this.transform.position, Vector3.right, rotation.Value.x);
        this.transform.RotateAround(this.transform.position, Vector3.up, rotation.Value.y);
    }

    private void UpdateSensivity(params float[] args)
    {
        this.sensivity = args[0];
    }
}
