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
                }
            }
        }
    }

    private int Depth(int x, int y, int z)
    {
        return Mathf.Min(x+1, y+1, z+1, size - x, size - y, size - z);
    }
}
