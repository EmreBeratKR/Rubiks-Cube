using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    public const float DragThreshold_InPixels = 50f;

    [SerializeField] private RubiksCube rubiksCube;
    [SerializeField] private Camera _camera;
    [SerializeField] private float speed;
    private Vector3 lastPos;
    private Cubie targetCubie;
    private RaycastHit? hit;
    private Axis? normal;
    private Coroutine moveCoroutine;
    public bool isMoving => moveCoroutine != null;

    [SerializeField] private Transform[] vectors;
    private void Update()
    {
        if (isMoving)
        {
            lastPos = Input.mousePosition;
            targetCubie = null;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            lastPos = Input.mousePosition;
            targetCubie = CubieRaycaster.Instance.GetTarget(out hit, out normal);
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (targetCubie == null) return;

            var rotAxis = rotationAxis;

            if (rotAxis == null) return;

            var _axis = CubieRaycaster.Instance.GetAxis(rotAxis.Value);

            var moveInfo = GetMoveInfo(targetCubie, rotAxis.Value, _axis.Value);
            
            moveCoroutine = StartCoroutine(Move_Co(moveInfo));
        }
    }

    private Vector3? rotationAxis
    {
        get
        {
            if (normal == null) return null;

            if (normal == Axis.X) return GetRotationAxis(rubiksCube.transform.up, rubiksCube.transform.forward);

            if (normal == Axis.Y) return GetRotationAxis(rubiksCube.transform.right, rubiksCube.transform.forward);

            if (normal == Axis.Z) return GetRotationAxis(rubiksCube.transform.right, rubiksCube.transform.up);

            return null;
        }
    }

    private bool IsReversed(Vector3 normal, Axis axis)
    {
        if (axis == Axis.X)
        {
            if (normal == -rubiksCube.transform.up) return true;

            if (normal == rubiksCube.transform.forward) return true;
        }

        if (axis == Axis.Y)
        {
            if (normal == rubiksCube.transform.right) return true;

            if (normal == -rubiksCube.transform.forward) return true;
        }

        if (axis == Axis.Z)
        {
            if (normal == -rubiksCube.transform.right) return true;

            if (normal == rubiksCube.transform.up) return true;
        }

        return false;
    }

    private Vector3? GetRotationAxis(Vector3 first, Vector3 second)
    {
        var firstAxis = hit.Value.point + first;
        var secondAxis = hit.Value.point + second;
        
        var wp_firstAxis = _camera.WorldToScreenPoint(firstAxis);
        var wp_secondAxis = _camera.WorldToScreenPoint(secondAxis);

        var currentMousePos = Input.mousePosition;

        var dotFirst = Vector3.Dot(wp_firstAxis - lastPos, currentMousePos - lastPos);
        var dotSecond = Vector3.Dot(wp_secondAxis - lastPos, currentMousePos - lastPos);

        if ((Input.mousePosition - lastPos).magnitude < DragThreshold_InPixels) return null;

        Debug.Log(dotFirst + " | " + dotSecond);

        if (Mathf.Abs(dotFirst) > Mathf.Abs(dotSecond))
        {
            return second * (dotFirst > 0f ? 1f : -1f);
        }
        else
        {
            return first * (dotSecond > 0f ? 1f : -1f);
        }
    }

    private MoveInfo GetMoveInfo(Cubie origin, Vector3 singedAxis, Axis axis)
    {
        var result = MoveInfo.Default;

        result.axis = singedAxis;
        result.isReversed = IsReversed(hit.Value.normal, axis);
        
        if (axis == Axis.X)
        {
            for (int y = 0; y < RubiksCube.size; y++)
            {
                for (int z = 0; z < RubiksCube.size; z++)
                {
                    var cubie = rubiksCube.cubies[origin.gridPosition.x, y, z];

                    if (cubie == null) continue;

                    result.cubies.Add(cubie);
                }
            }
        }

        else if (axis == Axis.Y)
        {
            for (int x = 0; x < RubiksCube.size; x++)
            {
                for (int z = 0; z < RubiksCube.size; z++)
                {
                    var cubie = rubiksCube.cubies[x, origin.gridPosition.y, z];

                    if (cubie == null) continue;

                    result.cubies.Add(cubie);
                }
            }
        }

        else if (axis == Axis.Z)
        {
            for (int x = 0; x < RubiksCube.size; x++)
            {
                for (int y = 0; y < RubiksCube.size; y++)
                {
                    var cubie = rubiksCube.cubies[x, y, origin.gridPosition.z];

                    if (cubie == null) continue;

                    result.cubies.Add(cubie);
                }
            }
        }

        return result;
    }

    private IEnumerator Move_Co(MoveInfo moveInfo)
    {
        float rotated = 0f;
        while (true)
        {    
            float angle = speed * Time.deltaTime;
            float lastRotated = rotated;
            rotated += angle;
            if (rotated >= 90f)
            {
                angle = 90f - lastRotated;
                rotated = 0f;
                foreach (var cubie in moveInfo.cubies)
                {
                    cubie.transform.RotateAround(rubiksCube.transform.position, moveInfo.axis, angle * (moveInfo.isReversed ? -1f : 1f));
                }
                break;
            }
            foreach (var cubie in moveInfo.cubies)
            {
                cubie.transform.RotateAround(rubiksCube.transform.position, moveInfo.axis, angle * (moveInfo.isReversed ? -1f : 1f));
            }

            yield return 0;
        }

        foreach (var cubie in moveInfo.cubies)
        {
            cubie.SetPosition(cubie.GetGridPosition());
            rubiksCube.cubies[cubie.gridPosition.x, cubie.gridPosition.y, cubie.gridPosition.z] = cubie;
        }

        moveCoroutine = null;
    }
}

public enum Axis { X, Y, Z }

public struct MoveInfo
{
    public bool isReversed;
    public Vector3 axis;
    public List<Cubie> cubies;

    public static MoveInfo Default => new MoveInfo
    {
        axis = Vector3.zero,
        cubies = new List<Cubie>()
    };
}