using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubieRaycaster : Scenegleton<CubieRaycaster>
{
    private static Camera _camera;

    [SerializeField] private LayerMask layerMask;

    public Cubie GetTarget(out Vector3? axis)
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
        {
            axis = hit.normal;
            return hit.collider.GetComponent<Cubie>();
        }
        axis = null;
        return null;
    }

    public override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }
}
