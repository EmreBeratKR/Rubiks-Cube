using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cubie : MonoBehaviour
{
    [field: SerializeField]
    public CubieFaces faces { get; private set; }
    
    public Vector3Int gridPosition { get; private set; }
    public float size { get => transform.localScale.x; set => transform.localScale = Vector3.one * value; }

    
    public void SetPosition(int x, int y, int z)
    {
        gridPosition = new Vector3Int(x, y, z);
    }

    public void SetPosition(Vector3Int gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public Vector3Int GetGridPosition()
    {
        var aprx = (this.transform.localPosition + RubiksCube.offset) / RubiksCube.cubieSize;
        return new Vector3Int(Mathf.RoundToInt(aprx.x), Mathf.RoundToInt(aprx.y), Mathf.RoundToInt(aprx.z));
    }

    public void Colorize()
    {
        faces.top.ApplyColor(Face.ColorCode.YELLOW);
        faces.bottom.ApplyColor(Face.ColorCode.WHITE);
        faces.front.ApplyColor(Face.ColorCode.RED);
        faces.back.ApplyColor(Face.ColorCode.ORANGE);
        faces.right.ApplyColor(Face.ColorCode.GREEN);
        faces.left.ApplyColor(Face.ColorCode.BLUE);
    }
}

[System.Serializable]
public struct CubieFaces
{
    public const int Count = 6;

    public Face top;
    public Face bottom;
    public Face right;
    public Face left;
    public Face front;
    public Face back;

    public Face this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return top;
                case 1:
                    return bottom;
                case 2:
                    return right;
                case 3:
                    return left;
                case 4:
                    return front;
                case 5:
                    return back;
                default:
                    return null;
            }
        }
    }
}