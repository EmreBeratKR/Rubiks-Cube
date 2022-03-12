using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    [field: SerializeField] public ColorCode colorCode { get; private set; }


    public void ApplyColor(ColorCode colorCode)
    {
        this.colorCode = colorCode;
        meshRenderer.material = Colorizer.ToMaterial(colorCode);
    }

    

    public enum ColorCode { RED, ORANGE, GREEN, BLUE, YELLOW, WHITE, NONE };
}
