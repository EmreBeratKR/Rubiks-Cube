using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFitter : MonoBehaviour
{
    private const float MAX_ZOOM = 1.5f;
    private const float MIN_ZOOM = 10f;

    private static Camera _camera;

    [SerializeField, Range(MAX_ZOOM, MIN_ZOOM)] private float shrinkness;
    [SerializeField] private float zoomSensivity;
    [SerializeField] private bool invertZoom;


    private void Awake()
    {
        _camera = this.GetComponent<Camera>();
    }

    private void Start()
    {
        Fit();
    }

    private void OnEnable()
    {
        EventSystem.OnCubeSizeChanged += OnCubeSizeChanged;
        EventSystem.OnZoomSensivityChanged += UpdateSensivity;
        EventSystem.OnInvertZoomToggled += UpdateInvert;
    }

    private void OnDisable()
    {
        EventSystem.OnCubeSizeChanged -= OnCubeSizeChanged;
        EventSystem.OnZoomSensivityChanged -= UpdateSensivity;
        EventSystem.OnInvertZoomToggled -= UpdateInvert;
    }

    private void Update()
    {
        HandleZoom();
    }

    private void HandleZoom()
    {
        if (!InputChecker.Instance.isEnabled) return;

        float scroll = Input.mouseScrollDelta.y;

        if (scroll == 0) return;

        shrinkness += scroll * zoomSensivity * (invertZoom ? 1 : -1);

        shrinkness =  Mathf.Clamp(shrinkness, MAX_ZOOM, MIN_ZOOM);

        Fit();
    }

    private void UpdateSensivity(params float[] args)
    {
        this.zoomSensivity = args[0];
    }

    private void UpdateInvert(params bool[] args)
    {
        this.invertZoom = args[0];
    }

    private void OnCubeSizeChanged(params int[] size)
    {
        Fit();
    }

    private void Fit()
    {
        _camera.transform.position = Vector3.back * (RubiksCube.Instance.size * RubiksCube.cubieSize * shrinkness);
    }
}
