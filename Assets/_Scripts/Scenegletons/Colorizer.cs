using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorizer : Scenegleton<Colorizer>
{
    [SerializeField] private FaceMaterials faceMaterials;


    public static Material ToMaterial(Face.ColorCode code)
    {
        switch (code)
        {
            case Face.ColorCode.RED:
                return Instance.faceMaterials.red;
            case Face.ColorCode.ORANGE:
                return Instance.faceMaterials.orange;
            case Face.ColorCode.GREEN:
                return Instance.faceMaterials.green;
            case Face.ColorCode.BLUE:
                return Instance.faceMaterials.blue;
            case Face.ColorCode.YELLOW:
                return Instance.faceMaterials.yellow;
            case Face.ColorCode.WHITE:
                return Instance.faceMaterials.white;
            case Face.ColorCode.NONE:
                return Instance.faceMaterials.none;
            default:
                return null;
        }
    }
}

[System.Serializable]
public struct FaceMaterials
{
    public Material red;
    public Material orange;
    public Material green;
    public Material blue;
    public Material yellow;
    public Material white;
    public Material none;
}