using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubieRaycaster : Scenegleton<CubieRaycaster>
{
    private const float MAX_DISTANCE = 500f;

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


    Vector3 _origin;
    RaycastHit _hit;
    Axis? _axis;
    private void OnDrawGizmos()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _origin = Input.mousePosition;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, MAX_DISTANCE, layerMask))
            {
                _axis = GetAxis(hit.normal);
                _hit = hit;
            }
        }
        else if (Input.GetMouseButton(0))
        {   
            if (_axis == Axis.Y)
            {
                var firstAxis = _hit.point + rubiksCube.transform.right;
                var secondAxis = _hit.point + rubiksCube.transform.forward;
                
                var wp_firstAxis = _camera.WorldToScreenPoint(firstAxis);
                var wp_secondAxis = _camera.WorldToScreenPoint(secondAxis);

                var currentMousePos = Input.mousePosition;

                var dotFirst = Vector3.Dot(wp_firstAxis - _origin, currentMousePos - _origin);
                var dotSecond = Vector3.Dot(wp_secondAxis - _origin, currentMousePos - _origin);

                Debug.Log(dotFirst + " - " + dotSecond);

                if ((Input.mousePosition - _origin).magnitude < CubeMover.DragThreshold_InPixels) return;

                if (Mathf.Abs(dotFirst) > Mathf.Abs(dotSecond))
                {
                    Gizmos.color = Color.red;
                    if (dotFirst > 0f) Gizmos.DrawLine(_hit.point, _hit.point + rubiksCube.transform.right);
                    else if (dotFirst < 0f) Gizmos.DrawLine(_hit.point, _hit.point - rubiksCube.transform.right);
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawSphere(_hit.point, 0.1f);
                }
                else
                {
                    Gizmos.color = Color.blue;
                    if (dotSecond > 0f) Gizmos.DrawLine(_hit.point, _hit.point + rubiksCube.transform.forward);
                    else if (dotSecond < 0f) Gizmos.DrawLine(_hit.point, _hit.point - rubiksCube.transform.forward);
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawSphere(_hit.point, 0.1f);
                }
            }
        }
    }
}
