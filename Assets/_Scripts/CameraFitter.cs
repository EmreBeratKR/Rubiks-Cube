using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFitter : MonoBehaviour
{
    private static Camera _camera;

    [SerializeField] private float shrinkness;


    private void Awake()
    {
        _camera = this.GetComponent<Camera>();
    }

    private void OnEnable()
    {
        EventSystem.OnCubeSizeChanged += OnCubeSizeChanged;
    }

    private void OnDisable()
    {
        EventSystem.OnCubeSizeChanged -= OnCubeSizeChanged;
    }

    private void OnCubeSizeChanged(params int[] size)
    {
        Fit(size[0]);
    }

    private void Fit(int size)
    {
        _camera.transform.position = Vector3.back * (size * RubiksCube.cubieSize * shrinkness);
    }
}
