using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksCube : Scenegleton<RubiksCube>
{
    public const int MAX_DEPTH = 1;
    public const float cubieSize = 1f;

    [SerializeField] private Cubie cubiePrefab;
    private Transform cubiesParent;
    
    public static Vector3 offset => Vector3.one * (RubiksCube.Instance.size-1) * cubieSize * 0.5f;

    private List<Cubie> cubiesList;
    public static Cubie randomCubie => RubiksCube.Instance.cubiesList[Random.Range(0, RubiksCube.Instance.cubiesList.Count)];

    public Cubie[,,] cubies;
    public int size;


    private void Start()
    {
        Generate();
    }

    private void OnEnable()
    {
        EventSystem.OnCubeSizeChanged += OnCubeSizeChanged;
    }

    private void OnDisable()
    {
        EventSystem.OnCubeSizeChanged -= OnCubeSizeChanged;
    }

    private void OnCubeSizeChanged(params int[] size)
    {
        EventSystem.CubeGenerateStarted();
        if (size.Length != 0) this.size = size[0];
        Destruct();
        Generate();
    }

    public void ResetCube()
    {
        OnCubeSizeChanged();
    }

    private void Generate()
    {
        StartCoroutine(Generate_Co());
    }

    private IEnumerator Generate_Co()
    {
        cubiesParent = new GameObject("Cubies").transform;
        cubiesParent.parent = this.transform;
        cubiesParent.localPosition = Vector3.zero;
        cubiesParent.localEulerAngles = Vector3.zero;
        cubiesParent.localScale = Vector3.one;

        CubeMap.Instance.Init();

        cubiesList = new List<Cubie>();
        cubies = new Cubie[size, size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    if (Depth(x, y, z) > MAX_DEPTH) continue;

                    Cubie cubie = Instantiate(cubiePrefab, cubiesParent);
                    cubie.transform.localPosition = new Vector3(x, y, z) * cubieSize - offset;
                    cubie.transform.localRotation = Quaternion.identity;
                    cubie.transform.localScale = Vector3.one * cubieSize;
                    cubie.transform.parent = cubiesParent;
                    cubie.name = $"Cubie ({x}, {y}, {z})";
                    cubies[x, y, z] = cubie;
                    cubie.SetPosition(x, y, z);
                    cubiesList.Add(cubie);

                    if (x != 0) Destroy(cubie.faces.left.gameObject);

                    if (x != size-1) Destroy(cubie.faces.right.gameObject);

                    if (y != 0) Destroy(cubie.faces.bottom.gameObject);

                    if (y != size-1) Destroy(cubie.faces.top.gameObject);

                    if (z != 0) Destroy(cubie.faces.front.gameObject);

                    if (z != size-1) Destroy(cubie.faces.back.gameObject);
                }
            }
            yield return 0;
        }
        EventSystem.CubeGenerated();
    }

    private void Destruct()
    {
        if (cubiesParent == null) return;

        Destroy(cubiesParent.gameObject);
        cubiesParent = null;
    }

    private int Depth(int x, int y, int z)
    {
        return Mathf.Min(x+1, y+1, z+1, size - x, size - y, size - z);
    }
}
