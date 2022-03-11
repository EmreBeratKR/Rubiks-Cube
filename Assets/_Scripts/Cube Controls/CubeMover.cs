using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    [SerializeField] private RubiksCube rubiksCube;
    [SerializeField] private float speed;
    private Cubie targetCubie;
    private Axis? axis;
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

            var moves = GetMovables(targetCubie, axis.Value);
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

    private List<MoveInfo> GetMovables(Cubie origin, Axis axis)
    {
        var result = new List<MoveInfo>();

        if (axis == Axis.Y)
        {
            var move_0 = MoveInfo.Default;
            move_0.axis = this.transform.right;
            for (int y = 0; y < RubiksCube.size; y++)
            {
                for (int z = 0; z < RubiksCube.size; z++)
                {
                    var cubie = rubiksCube.cubies[origin.gridPosition.x, y, z];

                    if (cubie == null) continue;

                    move_0.cubies.Add(cubie);
                }
            }
            result.Add(move_0);

            var move_1 = MoveInfo.Default;
            move_1.axis = this.transform.forward;
            for (int y = 0; y < RubiksCube.size; y++)
            {
                for (int x = 0; x < RubiksCube.size; x++)
                {
                    var cubie = rubiksCube.cubies[x, y, origin.gridPosition.z];

                    if (cubie == null) continue;

                    move_1.cubies.Add(cubie);
                }
            }
            result.Add(move_1);
        }

        else if (axis == Axis.X)
        {
            var move_0 = MoveInfo.Default;
            move_0.axis = this.transform.up;
            for (int x = 0; x < RubiksCube.size; x++)
            {
                for (int z = 0; z < RubiksCube.size; z++)
                {
                    var cubie = rubiksCube.cubies[x, origin.gridPosition.y, z];

                    if (cubie == null) continue;

                    move_0.cubies.Add(cubie);
                }
            }
            result.Add(move_0);

            var move_1 = MoveInfo.Default;
            move_1.axis = this.transform.forward;
            for (int x = 0; x < RubiksCube.size; x++)
            {
                for (int y = 0; y < RubiksCube.size; y++)
                {
                    var cubie = rubiksCube.cubies[x, y, origin.gridPosition.z];

                    if (cubie == null) continue;

                    move_1.cubies.Add(cubie);
                }
            }
            result.Add(move_1);
        }

        else if (axis == Axis.Z)
        {
            var move_0 = MoveInfo.Default;
            move_0.axis = this.transform.right;
            for (int z = 0; z < RubiksCube.size; z++)
            {
                for (int y = 0; y < RubiksCube.size; y++)
                {
                    var cubie = rubiksCube.cubies[origin.gridPosition.x, y, z];

                    if (cubie == null) continue;

                    move_0.cubies.Add(cubie);
                }
            }
            result.Add(move_0);

            var move_1 = MoveInfo.Default;
            move_1.axis = this.transform.up;
            for (int z = 0; z < RubiksCube.size; z++)
            {
                for (int x = 0; x < RubiksCube.size; x++)
                {
                    var cubie = rubiksCube.cubies[x, origin.gridPosition.y, z];

                    if (cubie == null) continue;

                    move_1.cubies.Add(cubie);
                }
            }
            result.Add(move_1);
        }

        return result;
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