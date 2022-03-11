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

    private void Start()
    {
        Fit();
    }

    private void Fit()
    {
        _camera.transform.position = Vector3.back * (RubiksCube.size * RubiksCube.cubieSize * shrinkness);
    }
}
