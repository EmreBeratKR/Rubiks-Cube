using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMap : Scenegleton<CubeMap>
{
    public const float RAYCAST_OFFSET_RATE = 0.55f; // 0.5f< (inside cube), 0.5f= (cube surface), 0.5f> (outside cube)

    public CubeMapRaycaster top;
    public CubeMapRaycaster bottom;
    public CubeMapRaycaster right;
    public CubeMapRaycaster left;
    public CubeMapRaycaster front;
    public CubeMapRaycaster back;

    public bool IsSolved
    {
        get
        {
            if (!top.IsSolved) return false;
            if (!bottom.IsSolved) return false;
            if (!right.IsSolved) return false;
            if (!left.IsSolved) return false;
            if (!front.IsSolved) return false;
            if (!back.IsSolved) return false;
            return true;
        }
    }


    private void OnEnable()
    {
        EventSystem.OnCubeSideRotated += CheckCube;
    }

    private void OnDisable()
    {
        EventSystem.OnCubeSideRotated -= CheckCube;
    }

    public void Init()
    {
        top.transform.position = top.transform.position.normalized * RubiksCube.size * RubiksCube.cubieSize * RAYCAST_OFFSET_RATE;
        bottom.transform.position = bottom.transform.position.normalized * RubiksCube.size * RubiksCube.cubieSize * RAYCAST_OFFSET_RATE;
        right.transform.position = right.transform.position.normalized * RubiksCube.size * RubiksCube.cubieSize * RAYCAST_OFFSET_RATE;
        left.transform.position = left.transform.position.normalized * RubiksCube.size * RubiksCube.cubieSize * RAYCAST_OFFSET_RATE;
        front.transform.position = front.transform.position.normalized * RubiksCube.size * RubiksCube.cubieSize * RAYCAST_OFFSET_RATE;
        back.transform.position = back.transform.position.normalized * RubiksCube.size * RubiksCube.cubieSize * RAYCAST_OFFSET_RATE;
    }

    private void CheckCube()
    {
        if (IsSolved) EventSystem.CubeSolved();
    }
}
