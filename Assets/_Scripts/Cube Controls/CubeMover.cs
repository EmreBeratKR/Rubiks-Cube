using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    [SerializeField] private RubiksCube rubiksCube;
    [SerializeField] private float speed;
    private Cubie targetCubie;
    private Vector3? axis;
    private Coroutine moveCoroutine;
    public bool isMoving => moveCoroutine != null;


    private void Update()
    {
        if (isMoving) return;

        if (Input.GetMouseButtonDown(0))
        {
            targetCubie = CubieRaycaster.Instance.GetTarget(out axis);
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (targetCubie == null) return;

            var moves = rubiksCube.GetMovables(targetCubie, GetAxis().Value);
            MoveInfo moveInfo =  MoveInfo.Default;
            bool moved = false;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                moveInfo = moves[0];
                moved = true;
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                moveInfo = moves[1];
                moved = true;
            }

            if (!moved) return;
            
            moveCoroutine = StartCoroutine(Move_Co(moveInfo, Input.GetKey(KeyCode.LeftShift) ? -1 : 1));
        }
    }

    private Axis? GetAxis()
    {
        if (axis == null) return null;

        if (axis == this.transform.up) return Axis.Y;

        if (axis == -this.transform.up) return Axis.Y;

        if (axis == this.transform.right) return Axis.X;

        if (axis == -this.transform.right) return Axis.X;

        if (axis == -this.transform.forward) return Axis.Z;

        if (axis == this.transform.forward) return Axis.Z;

        return null;
    }

    private IEnumerator Move_Co(MoveInfo moveInfo, int dir)
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
                    cubie.transform.RotateAround(rubiksCube.transform.position, moveInfo.axis, angle * dir);
                }
                break;
            }
            foreach (var cubie in moveInfo.cubies)
            {
                cubie.transform.RotateAround(rubiksCube.transform.position, moveInfo.axis, angle * dir);
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
    public Vector3 axis;
    public List<Cubie> cubies;

    public static MoveInfo Default => new MoveInfo
    {
        axis = Vector3.zero,
        cubies = new List<Cubie>()
    };
}