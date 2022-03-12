using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubieRaycaster : Scenegleton<CubieRaycaster>
{
    private const float MAX_DISTANCE = 1000f;

    [SerializeField] private RubiksCube rubiksCube;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask layerMask;

    public Cubie GetTarget(out RaycastHit? hit, out Axis? normal)
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit _hit, MAX_DISTANCE, layerMask))
        {
            hit = _hit;
            normal = GetAxis(_hit.normal);
            return _hit.collider.GetComponentInParent<Cubie>();
        }
        hit = null;
        normal = null;
        return null;
    }

    public Axis? GetAxis(Vector3 axis)
    {
        if (axis == null) return null;

        if (axis == rubiksCube.transform.up) return Axis.Y;

        if (axis == -rubiksCube.transform.up) return Axis.Y;

        if (axis == rubiksCube.transform.right) return Axis.X;

        if (axis == -rubiksCube.transform.right) return Axis.X;

        if (axis == -rubiksCube.transform.forward) return Axis.Z;

        if (axis == rubiksCube.transform.forward) return Axis.Z;

        return null;
    }
}
