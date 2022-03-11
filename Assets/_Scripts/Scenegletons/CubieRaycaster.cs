using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubieRaycaster : Scenegleton<CubieRaycaster>
{
    [SerializeField] private RubiksCube rubiksCube;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask layerMask;

    public Cubie GetTarget(out Axis? axis)
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
        {
            axis = GetAxis(hit.normal);
            return hit.collider.GetComponentInParent<Cubie>();
        }
        axis = null;
        return null;
    }

    private Axis? GetAxis(Vector3 axis)
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
