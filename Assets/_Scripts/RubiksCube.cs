using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksCube : MonoBehaviour
{
    public const int MAX_DEPTH = 1;
    public const int size = 13;
    public const float cubieSize = 1f;

    [SerializeField] private Cubie cubiePrefab;
    
    public static Vector3 offset => Vector3.one * (size-1) * cubieSize * 0.5f;

    public Cubie[,,] cubies;

    private void Start()
    {
        Generate();
    }

    public List<MoveInfo> GetMovables(Cubie origin, Axis axis)
    {
        var result = new List<MoveInfo>();

        if (axis == Axis.Y)
        {
            var move_0 = MoveInfo.Default;
            move_0.axis = this.transform.right;
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    var cubie = cubies[origin.gridPosition.x, y, z];

                    if (cubie == null) continue;

                    move_0.cubies.Add(cubie);
                }
            }
            result.Add(move_0);

            var move_1 = MoveInfo.Default;
            move_1.axis = this.transform.forward;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    var cubie = cubies[x, y, origin.gridPosition.z];

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
            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    var cubie = cubies[x, origin.gridPosition.y, z];

                    if (cubie == null) continue;

                    move_0.cubies.Add(cubie);
                }
            }
            result.Add(move_0);

            var move_1 = MoveInfo.Default;
            move_1.axis = this.transform.forward;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    var cubie = cubies[x, y, origin.gridPosition.z];

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
            for (int z = 0; z < size; z++)
            {
                for (int y = 0; y < size; y++)
                {
                    var cubie = cubies[origin.gridPosition.x, y, z];

                    if (cubie == null) continue;

                    move_0.cubies.Add(cubie);
                }
            }
            result.Add(move_0);

            var move_1 = MoveInfo.Default;
            move_1.axis = this.transform.up;
            for (int z = 0; z < size; z++)
            {
                for (int x = 0; x < size; x++)
                {
                    var cubie = cubies[x, origin.gridPosition.y, z];

                    if (cubie == null) continue;

                    move_1.cubies.Add(cubie);
                }
            }
            result.Add(move_1);
        }

        return result;
    }

    private void Generate()
    {
        cubies = new Cubie[size, size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    if (Depth(x, y, z) > MAX_DEPTH) continue;

                    Cubie cubie = Instantiate(cubiePrefab, transform);
                    cubie.transform.localPosition = new Vector3(x, y, z) * cubieSize - offset;
                    cubie.transform.localRotation = Quaternion.identity;
                    cubie.transform.localScale = Vector3.one * cubieSize;
                    cubie.transform.parent = transform;
                    cubie.name = $"Cubie ({x}, {y}, {z})";
                    cubies[x, y, z] = cubie;
                    cubie.SetPosition(x, y, z);
                    cubie.Colorize();
                }
            }
        }
    }

    private int Depth(int x, int y, int z)
    {
        return Mathf.Min(x+1, y+1, z+1, size - x, size - y, size - z);
    }
}
